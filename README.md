# PrCollector

tools to collect and edit venus pr for the last week


## debug

```sh
dotnet build -t:Run -f net6.0-maccatalyst 
```


## release package 

```sh
dotnet build -f:net6.0-maccatalyst -c:Release /p:CreatePackage=true
```