Swagger доступен: https://localhost:7032/swagger/index.html



Управление счетами (/api/accounts)

GET	/api/accounts	Получить список всех счетов

GET	/api/accounts/{id}	Получить счет по ID

POST	/api/accounts	Создать новый счет

PATCH	/api/accounts/{id}	Обновить счет (частично)

DELETE	/api/accounts/{id}	Удалить счет

GET	/api/accounts/owner/{ownerId}	Получить счета клиента

GET	/api/accounts/{id}/exists	Проверить существование счета

Управление транзакциями (/api/transactions)

POST	/api/transactions	Создать транзакцию

GET	/api/transactions/{id}	Получить транзакцию по ID

GET	/api/transactions/account/{accountId}	Получить транзакции по счету

POST	/api/transactions/transfer	Выполнить перевод между счетами

GET	/api/transactions/account/{accountId}/statement	Получить выписку по счету



хз что ещё сюда нужно добавить

