# Stage 1 (Build Stage)
FROM microsoft/dotnet:2.2-sdk as builder
LABEL author="Jawad Hasan"
WORKDIR /source

COPY . . 
RUN dotnet restore

# copies the rest of your code
COPY . .
RUN dotnet publish --output /app/ --configuration Release

# Stage 2
#FROM microsoft/dotnet:2.2-sdk
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "SampleApi.Web.dll"]


# docker build -t dotnetcore-sampleapi -f dotnetcore.prod.dockerfile .
# docker run -p 8080:80 dotnetcore-sampleapi

