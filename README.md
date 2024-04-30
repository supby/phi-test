# phi-test

[Task description](https://github.com/supby/phi-test/blob/main/doc/task.pdf)


## How to run

My setup:
- Ubuntu linux
- .NET Core SDK: `8.0.103` [/usr/lib/dotnet/sdk]

Run following command in the root directory of the project:
```
dotnet build . && dotnet run --project ./src/Phi.Api/
```

Run tests:
```
dotnet test
```

The applications Swagger UI will be accessible on `http://localhost:5214/swagger/index.html`

## Considerations

### Cache

In order to not overload HackerNews API, In-memory cache was used for test purposes only. The In-mem approach is not horizontally scalable so in real scenarious some external key/value storage should be used. (redis and so on)

### beststories.json endpoint already sorted in descending order

I did not find any confirmation in docs that `beststories.json` endpoint returns ids sorted in descending by score order but according to tests looks like it is true. My assumption is beststories.json returns ids in descending by score order and I can load story details only for the first `N` ids.

In case it is not true it needs to load all stories to be able to sort them, which would require a warm-up cache by first requests after application start.
    

## TODOs

1. Adding AutoMapper configuration to map from HackerNews API model `Phi.Model.Client.Story` to API model `Phi.Model.API.Story`
2. Dockerize the application to not depend on the environment.
3. Think about the cache invalidation mechanism in case data is changed.
4. Adding sturtup check in case if configuration is not valid.
5. Adding more unit tests.

