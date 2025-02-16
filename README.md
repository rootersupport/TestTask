# TestTask

Веб-API для управления пользователями с использованием .NET 6.0, PostgreSQL и Entity Framework Core

## Установка

1. **Клонирование**:
   ```bash
   git clone https://github.com/username/TestTask.git
   ```

2. **Пакеты**:
   ```bash
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 6.0.0
   dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 6.0.0
   dotnet add package Microsoft.EntityFrameworkCore --version 6.0.0
   dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.0
   dotnet add package Microsoft.EntityFrameworkCore.Tools --version 6.0.0
   dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 6.0.0
   dotnet add package Swashbuckle.AspNetCore --version 6.2.3
   dotnet add package System.IdentityModel.Tokens.Jwt --version 6.15.0
   ```

3. **База данных**:
   - Создайте базу данных.
   - Обновите строку подключения в `appsettings.json`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=dbname;Username=postgres;Password=password"
     }
     ```

4. **Миграции**:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Запуск**:
   ```bash
   dotnet run
   ```
   API будет доступно по адресу: `https://localhost:7194`.

## Структура API

### Аутентификация

#### `POST /api/Auth/login`:
Аутентификация пользователя.

**Тело запроса:**
```json
{
  "login": "string",
  "password": "string"
}
```

**Ответ:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Пользователи

#### `GET /api/Users`:
Получить список всех пользователей.
- Требуется роль `Admin`.

#### `GET /api/Users/{id}`:
Получить информацию о пользователе по ID.
- Требуется аутентификация.

#### `POST /api/Users`:
Добавить нового пользователя.

**Тело запроса:**
```json
{
  "login": "string",
  "password": "string",
  "userGroupId": 1,
  "userStateId": 1
}
```
- Требуется роль `Admin`.

#### `PUT /api/Users/{id}`:
Обновить информацию о пользователе.

**Тело запроса:**
```json
{
  "login": "string",
  "password": "string",
  "userGroupId": 1,
  "userStateId": 1
}
```
- Требуется роль `Admin`.

#### `DELETE /api/Users/{id}`:
Заблокировать пользователя (установить статус `Blocked`).
- Требуется роль `Admin`. 
