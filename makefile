run:
	cd sample/Books.Web/ && dotnet run
client:
	cd sample/Books.Console/ && dotnet run

build:
	dotnet build GoodCrud.sln
clean:
	dotnet clean GoodCrud.sln

# NAME=InitCreate make db_migrate
db_migrate:
	cd sample/Books.Data && dotnet ef migrations add $$NAME --startup-project ../Books.Web
db_update:
	cd sample/Books.Data && dotnet ef database update --startup-project ../Books.Web


.PHONY: test
test:
	cd test/GoodCrud.Domain.Tests && dotnet test
	cd test/GoodCrud.Data.Tests && dotnet test
	cd test/GoodCrud.Application.Tests && dotnet test
	cd test/GoodCrud.Web.Tests && dotnet test
