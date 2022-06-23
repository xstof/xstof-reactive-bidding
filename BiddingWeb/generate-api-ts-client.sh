SWAGGER_URL=https://localhost:7294/swagger/index.html
# run executable 'dotnet' in the background then echo the PID do more work and shut it down

dotnet run --project ../BiddingAPI/BiddingAPI.csproj &

sleep 10

# curl -k https://localhost:7294/swagger/v1/swagger.json -o swagger.json

npx -y swagger-typescript-api -o ./spa-client/src/biddingclient -p https://localhost:7294/swagger/v1/swagger.json  --modular --disableStrictSSL -n AuctionsClient.ts
# rm swagger.json

kill %- # kill the last job (dotnet run in this case)
