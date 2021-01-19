FROM mcr.microsoft.com/dotnet/core/sdk:3.1

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# copy the compiled code (published into the ./app directory by the Build step) into the container's root.
COPY ./app/* ./

ENTRYPOINT ["dotnet", "bstest5.dll"]
