benchmark:
	dotnet run --project "benchmarks/Benchmarks/Benchmarks.csproj" -c Release -f net6.0 net7.0 --runtimes net6.0 net7.0

test:
	dotnet test "tests/CodEaisy.TinySaas.Tests" \
		/p:CollectCoverage=true \
		/p:CoverletOutputFormat=\"json,lcov,opencover\" \
		/p:ExcludeByFile=\"**/CodEaisy.TinySaas/Ensure.cs,**/CodEaisy.TinySaas/Internals/MultiTenantContainer.cs,**/CodEaisy.TinySaas.Auth/Authorization/AuthorizationMiddlewareResultHandler.cs\" \
		/p:CoverletOutput=\"../../coverage/\" \
		/p:MergeWith=\"../../coverage/coverage.net7.0.json\"

	rm coverage/coverage.net7.0.json

visualize_coverage:
	make test
	make open_coverage_in_browser

open_coverage_in_browser:
	reportgenerator "-reports:coverage/coverage.opencover.xml" \
		"-reporttypes:Html" "-targetdir:./coverage/html"
	open coverage/html/index.html

analyze_pr:
	dotnet sonarscanner begin /k:"CodEaisy_TinySaas" /o:"codeaisy" \
          /d:sonar.login="${SONAR_TOKEN}" \
          /d:sonar.cs.opencover.reportsPaths="./coverage/coverage.opencover.xml" \
		  /d:sonar.coverage.exclusions="**/CodEaisy.TinySaas/Ensure.cs,**/CodEaisy.TinySaas/Internals/MultiTenantContainer.cs,**/CodEaisy.TinySaas.Auth/Authorization/AuthorizationMiddlewareResultHandler.cs" \
          /d:sonar.pullrequest.key=${CHANGE_ID} \
          /d:sonar.host.url="https://sonarcloud.io"
	dotnet build src
	dotnet sonarscanner end /d:sonar.login="${SONAR_TOKEN}"

analyze_br:
	dotnet sonarscanner begin /k:"CodEaisy_TinySaas" /o:"codeaisy" \
          /d:sonar.login="${SONAR_TOKEN}" \
          /d:sonar.cs.opencover.reportsPaths="./coverage/coverage.opencover.xml" \
		  /d:sonar.coverage.exclusions="**/CodEaisy.TinySaas/Ensure.cs,**/CodEaisy.TinySaas/Internals/MultiTenantContainer.cs,**/CodEaisy.TinySaas.Auth/Authorization/AuthorizationMiddlewareResultHandler.cs" \
          /d:sonar.host.url="https://sonarcloud.io"
	dotnet build src
	dotnet sonarscanner end /d:sonar.login="${SONAR_TOKEN}"
