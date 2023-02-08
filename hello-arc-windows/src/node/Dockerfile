FROM mcr.microsoft.com/windows/servercore:ltsc2019

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# PATH isn't actually set in the Docker image, so we have to set it from within the container
RUN $newPath = ('C:\nodejs;{0}\Yarn\bin;{1}' -f $env:LOCALAPPDATA, $env:PATH); \
	Write-Host ('Updating PATH: {0}' -f $newPath); \
	[Environment]::SetEnvironmentVariable('PATH', $newPath, [EnvironmentVariableTarget]::Machine)
# doing this first to share cache across versions more aggressively

ENV NODE_VERSION 18.14.0
ENV NODE_SHA256 2e8f00da72f6bd993e3b980ff844b948baf936e1e67e3694a7a3e5f6f7c9beb4

RUN $url = ('https://nodejs.org/dist/v{0}/node-v{0}-win-x64.zip' -f $env:NODE_VERSION); \
	Write-Host ('Downloading {0} ...' -f $url); \
	Invoke-WebRequest -Uri $url -OutFile 'node.zip'; \
	\
	Write-Host ('Verifying sha256 ({0}) ...' -f $env:NODE_SHA256); \
	if ((Get-FileHash node.zip -Algorithm sha256).Hash -ne $env:NODE_SHA256) { throw 'SHA256 mismatch' }; \
	\
	Write-Host 'Expanding ...'; \
	Expand-Archive node.zip -DestinationPath C:\; \
	\
	Write-Host 'Renaming ...'; \
	Rename-Item -Path ('C:\node-v{0}-win-x64' -f $env:NODE_VERSION) -NewName 'C:\nodejs'; \
	\
	Write-Host 'Removing ...'; \
	Remove-Item node.zip -Force; \
	\
	Write-Host 'Verifying ("node --version") ...'; \
	node --version; \
	Write-Host 'Verifying ("npm --version") ...'; \
	npm --version; \
	\
	Write-Host 'Complete.'

ENV YARN_VERSION 1.22.17

# "It is recommended to install Yarn through the npm package manager" (https://classic.yarnpkg.com/en/docs/install)
RUN Write-Host 'Installing "yarn" ...'; \
	npm install --global ('yarn@{0}' -f $env:YARN_VERSION); \
	\
	Write-Host 'Verifying ("yarn --version") ...'; \
	yarn --version; \
	\
	Write-Host 'Complete.'

CMD [ "node" ]