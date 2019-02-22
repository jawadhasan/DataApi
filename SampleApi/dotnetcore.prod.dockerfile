# Stage 1 (Build Stage)
FROM microsoft/dotnet:2.2-sdk as builder
LABEL author="Jawad Hasan"
WORKDIR /source

#COPY  ./SampleApi.sln ./
COPY ./src/Sample/SampleApi.Core/SampleApi.Core.csproj  ./src/Sample/SampleApi.Core/SampleApi.Core.csproj
COPY ./src/Sample/SampleApi.Data/SampleApi.Data.csproj  ./src/Sample/SampleApi.Data/SampleApi.Data.csproj
COPY ./src/Sample/SampleApi.Web/SampleApi.Web.csproj  ./src/Sample/SampleApi.Web/SampleApi.Web.csproj
COPY ./test/SampleApi.Tests/SampleApi.Tests.csproj  ./test/SampleApi.Tests/SampleApi.Tests.csproj

RUN dotnet restore ./src/Sample/SampleApi.Web/SampleApi.Web.csproj

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

