#You need to set your context to the root of the repo (i.e. C:\git\SampleSolutions\)
# docker build ..\.. -f .\Dockerfile
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

# Copy the sshd_config file to the /etc/ssh/ directory
COPY src/ITDService/ssh_setup.sh /opt/ssh_setup.sh
COPY src/ITDService/startup.sh /app/

RUN apt-get update \
    && apt-get install -y --no-install-recommends openssh-server \
    && echo "root:Docker!" | chpasswd \
    && chmod u+x /opt/ssh_setup.sh \
    && chmod u+x /app/startup.sh \
    && /opt/ssh_setup.sh 

# Use Tini to manage a single child process and ensure any zombie processes produced from it are reaped & that signals are properly forwarded. 
ENV TINI_VERSION v0.19.0
ADD https://github.com/krallin/tini/releases/download/${TINI_VERSION}/tini /tini
RUN chmod 0555 /tini
RUN chmod 0555 /app/startup.sh

COPY src/ITDService/developmentcertificate.crt /usr/local/share/ca-certificates
RUN chmod 0644 /usr/local/share/ca-certificates/developmentcertificate.crt && update-ca-certificates

COPY src/ITDService/sshd_config /etc/ssh/

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/ITDService/ITDService.csproj", "src/ITDService/"]
RUN dotnet restore "src/ITDService/ITDService.csproj"
COPY . .
WORKDIR "/src/src/ITDService"

FROM build AS publish
RUN dotnet publish "ITDService.csproj" -c Release --no-restore -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY src/ITDService/Config /data/config
ENV CONFIGURATION__BASEPATH /data/config

# Open port 2222 for SSH access
EXPOSE 80 2222

ENTRYPOINT ["/tini", "--", "bash", "/app/startup.sh"]
