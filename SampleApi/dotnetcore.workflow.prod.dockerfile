# Stage 1 (Build Stage)
FROM microsoft/dotnet:2.2-sdk as builder
LABEL author="Jawad Hasan"
WORKDIR /source


COPY ./src/Workflow/Workflow.Core/Workflow.Core.csproj  ./src/Workflow/Workflow.Core/Workflow.Core.csproj
COPY ./src/Workflow/Workflow.Data/Workflow.Data.csproj  ./src/Workflow/Workflow.Data/Workflow.Data.csproj
COPY ./src/Workflow/Workflow.Web/Workflow.Web.csproj  ./src/Workflow/Workflow.Web/Workflow.Web.csproj

RUN dotnet restore ./src/Workflow/Workflow.Web/Workflow.Web.csproj

# copies the rest of your code
COPY . .
RUN dotnet publish --output /app/ --configuration Release

# Stage 2
#FROM microsoft/dotnet:2.2-sdk
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "Workflow.Web.dll"]


# docker build -t dotnetcore-workflowapi -f dotnetcore.workflow.prod.dockerfile .
# docker run -p 8090:80 dotnetcore-workflowapi

