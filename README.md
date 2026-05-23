# Dishmade API — Documentação Técnica de Integração

## 1. Visão geral

A **Dishmade API** é um backend em **.NET 9** para um SaaS de gestão de restaurantes. A API atende fluxos de cardápio digital, gestão de mesas, pedidos, histórico de vendas e dashboard operacional.

O projeto segue arquitetura em camadas:

```txt
dishmade-api
 └── src
     ├── dishmade.api
     ├── dishmade.application
     ├── dishmade.domain
     └── dishmade.infra
```

### Responsabilidade de cada camada

| Camada | Responsabilidade |
|---|---|
| `dishmade.api` | Controllers, middlewares, configuração HTTP, Swagger e entrada da aplicação. |
| `dishmade.application` | Casos de uso, CQRS, commands, queries, validações, contratos de repositório e paginação. |
| `dishmade.domain` | Entidades, enums e regras de negócio centrais. |
| `dishmade.infra` | Entity Framework Core, SQL Server, DbContext, mappings e repositórios. |

A API usa:

- **CQRS com MediatR** para separar comandos e consultas.
- **FluentValidation** para validação dos requests.
- **Entity Framework Core** com **SQL Server** para persistência.
- **Soft delete** para pratos e mesas.
- **Paginação e filtros** nas principais listagens.
- **Enum serializado como string** nas respostas e requests JSON.

---

## 2. URL base

Ambiente local:

```txt
http://localhost:5280
```

Swagger local:

```txt
http://localhost:5280/swagger
```

Todos os endpoints usam o prefixo:

```txt
/api
```

---

## 3. Convenções gerais da API

### 3.1 Formato JSON

A API recebe e retorna JSON.

Header recomendado:

```http
Content-Type: application/json
Accept: application/json
```

### 3.2 IDs

Todos os identificadores principais são `Guid`.

Exemplo:

```json
{
  "id": "274bc5e2-33bd-4b3d-8815-e577697139e4"
}
```

### 3.3 Datas

As datas são retornadas em formato ISO 8601.

Exemplo:

```json
{
  "createdAt": "2026-05-23T11:38:20.3334217",
  "updatedAt": "2026-05-23T11:44:07.3214596",
  "deliveredAt": "2026-05-23T12:01:00.0000000"
}
```

### 3.4 Status HTTP comuns

| Status | Significado |
|---|---|
| `200 OK` | Consulta realizada com sucesso. |
| `201 Created` | Recurso criado com sucesso. |
| `204 No Content` | Operação executada com sucesso sem corpo de resposta. |
| `400 Bad Request` | Erro de validação ou regra de negócio. |
| `404 Not Found` | Recurso não encontrado. |
| `500 Internal Server Error` | Erro interno não tratado. |

---

## 4. Formato de erro

### 4.1 Erro de validação

Exemplo:

```json
{
  "statusCode": 400,
  "message": "Erro de validação.",
  "errors": [
    {
      "field": "Name",
      "message": "O nome do prato é obrigatório."
    }
  ]
}
```

### 4.2 Erro de regra de negócio

Exemplo:

```json
{
  "statusCode": 400,
  "message": "Não é possível criar pedido para uma mesa ocupada."
}
```

### 4.3 Recurso não encontrado

Exemplo:

```json
{
  "statusCode": 404,
  "message": "Pedido não encontrado."
}
```

---

## 5. Paginação

As listagens principais retornam dados paginados.

### 5.1 Parâmetros padrão

| Parâmetro | Tipo | Padrão | Descrição |
|---|---:|---:|---|
| `pageNumber` | `int` | `1` | Página atual. |
| `pageSize` | `int` | `10` | Quantidade de itens por página. |

O backend normaliza os valores:

- `pageNumber <= 0` vira `1`.
- `pageSize <= 0` vira `10`.
- `pageSize > 100` vira `100`.

### 5.2 Resposta paginada

Exemplo:

```json
{
  "items": [],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 25,
  "totalPages": 3,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

### 5.3 Campos da resposta paginada

| Campo | Tipo | Descrição |
|---|---|---|
| `items` | `array` | Itens da página atual. |
| `pageNumber` | `int` | Página atual. |
| `pageSize` | `int` | Tamanho da página. |
| `totalCount` | `int` | Total de registros encontrados no filtro. |
| `totalPages` | `int` | Total de páginas disponíveis. |
| `hasPreviousPage` | `boolean` | Indica se existe página anterior. |
| `hasNextPage` | `boolean` | Indica se existe próxima página. |

---

## 6. Categorias do cardápio

Categorias agrupam os pratos do cardápio.

Exemplos:

- Hambúrgueres
- Bebidas
- Sobremesas
- Entradas

### 6.1 Criar categoria

```http
POST /api/categories
```

#### Request

```json
{
  "name": "Hambúrgueres",
  "description": "Categoria de hambúrgueres artesanais"
}
```

#### Response `201 Created`

```json
{
  "id": "6e90fa5f-88b1-48c0-aabd-4e3fc1bb9cc7"
}
```

#### Regras

- `name` é obrigatório.
- `name` deve ter no máximo 100 caracteres.
- `description` deve ter no máximo 500 caracteres.
- Não pode existir outra categoria com o mesmo nome.

---

### 6.2 Listar categorias

```http
GET /api/categories
```

#### Filtros

| Query param | Tipo | Descrição |
|---|---|---|
| `search` | `string` | Busca por nome ou descrição. |
| `isActive` | `boolean` | Filtra categorias ativas ou inativas. |
| `pageNumber` | `int` | Página. |
| `pageSize` | `int` | Tamanho da página. |

#### Exemplo

```http
GET /api/categories?search=bebida&isActive=true&pageNumber=1&pageSize=10
```

#### Response `200 OK`

```json
{
  "items": [
    {
      "id": "6e90fa5f-88b1-48c0-aabd-4e3fc1bb9cc7",
      "name": "Bebidas",
      "description": "Sucos, refrigerantes e águas",
      "isActive": true,
      "createdAt": "2026-05-23T10:00:00"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

---

## 7. Pratos

Pratos representam os itens vendidos no cardápio.

Cada prato pertence a uma categoria.

### 7.1 Criar prato

```http
POST /api/dishes
```

#### Request

```json
{
  "name": "Smash Burger",
  "description": "Pão brioche, carne smash, cheddar e molho especial.",
  "price": 29.9,
  "categoryId": "6e90fa5f-88b1-48c0-aabd-4e3fc1bb9cc7"
}
```

#### Response `201 Created`

```json
{
  "id": "c878392b-e6a5-4824-a2bb-7142ae970737"
}
```

#### Regras

- `name` é obrigatório.
- `name` deve ter no máximo 150 caracteres.
- `description` deve ter no máximo 1000 caracteres.
- `price` deve ser maior que zero.
- `categoryId` é obrigatório.
- A categoria precisa existir.
- Não pode existir outro prato com o mesmo nome.

---

### 7.2 Listar pratos

```http
GET /api/dishes
```

#### Filtros

| Query param | Tipo | Descrição |
|---|---|---|
| `search` | `string` | Busca por nome ou descrição. |
| `categoryId` | `Guid` | Filtra pratos de uma categoria específica. |
| `isAvailable` | `boolean` | Filtra pratos disponíveis ou indisponíveis. |
| `pageNumber` | `int` | Página. |
| `pageSize` | `int` | Tamanho da página. |

#### Exemplo

```http
GET /api/dishes?search=burger&isAvailable=true&pageNumber=1&pageSize=10
```

#### Response `200 OK`

```json
{
  "items": [
    {
      "id": "c878392b-e6a5-4824-a2bb-7142ae970737",
      "name": "Smash Burger",
      "description": "Pão brioche, carne smash, cheddar e molho especial.",
      "price": 29.9,
      "isAvailable": true,
      "categoryId": "6e90fa5f-88b1-48c0-aabd-4e3fc1bb9cc7",
      "categoryName": "Hambúrgueres",
      "createdAt": "2026-05-23T10:30:00",
      "updatedAt": null
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

---

### 7.3 Buscar prato por ID

```http
GET /api/dishes/{id}
```

#### Exemplo

```http
GET /api/dishes/c878392b-e6a5-4824-a2bb-7142ae970737
```

#### Response `200 OK`

```json
{
  "id": "c878392b-e6a5-4824-a2bb-7142ae970737",
  "name": "Smash Burger",
  "description": "Pão brioche, carne smash, cheddar e molho especial.",
  "price": 29.9,
  "isAvailable": true,
  "categoryId": "6e90fa5f-88b1-48c0-aabd-4e3fc1bb9cc7",
  "categoryName": "Hambúrgueres",
  "createdAt": "2026-05-23T10:30:00",
  "updatedAt": null
}
```

---

### 7.4 Atualizar prato

```http
PUT /api/dishes/{id}
```

#### Request

```json
{
  "name": "Smash Burger Duplo",
  "description": "Pão brioche, dois hambúrgueres smash, cheddar e molho especial.",
  "price": 36.9,
  "categoryId": "6e90fa5f-88b1-48c0-aabd-4e3fc1bb9cc7"
}
```

#### Response `204 No Content`

Sem corpo de resposta.

#### Regras

- O prato precisa existir.
- A categoria precisa existir.
- Não pode existir outro prato com o mesmo nome.
- `price` deve ser maior que zero.

---

### 7.5 Remover prato

```http
DELETE /api/dishes/{id}
```

#### Response `204 No Content`

Sem corpo de resposta.

#### Regra importante

A remoção de prato é feita via **soft delete**:

```txt
IsDeleted = true
IsAvailable = false
```

O prato deixa de aparecer nas listagens normais, mas continua preservado para histórico de pedidos e vendas.

---

## 8. Mesas

Mesas representam os locais físicos do restaurante onde pedidos podem ser abertos.

### 8.1 Criar mesa

```http
POST /api/tables
```

#### Request

```json
{
  "number": 2
}
```

#### Response `201 Created`

```json
{
  "id": "f988d730-5457-451e-adab-47028cf2c097"
}
```

#### Regras

- `number` deve ser maior que zero.
- Não pode existir outra mesa ativa com o mesmo número.

---

### 8.2 Listar mesas

```http
GET /api/tables
```

#### Filtros

| Query param | Tipo | Descrição |
|---|---|---|
| `number` | `int` | Filtra por número da mesa. |
| `isOccupied` | `boolean` | Filtra mesas ocupadas ou livres. |
| `pageNumber` | `int` | Página. |
| `pageSize` | `int` | Tamanho da página. |

#### Exemplo

```http
GET /api/tables?isOccupied=false&pageNumber=1&pageSize=10
```

#### Response `200 OK`

```json
{
  "items": [
    {
      "id": "f988d730-5457-451e-adab-47028cf2c097",
      "number": 2,
      "isOccupied": false,
      "createdAt": "2026-05-23T11:00:00",
      "updatedAt": null
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

---

### 8.3 Buscar mesa por ID

```http
GET /api/tables/{id}
```

#### Response `200 OK`

```json
{
  "id": "f988d730-5457-451e-adab-47028cf2c097",
  "number": 2,
  "isOccupied": false,
  "createdAt": "2026-05-23T11:00:00",
  "updatedAt": null
}
```

---

### 8.4 Atualizar mesa

```http
PUT /api/tables/{id}
```

#### Request

```json
{
  "number": 10
}
```

#### Response `204 No Content`

Sem corpo de resposta.

---

### 8.5 Ocupar mesa manualmente

```http
PATCH /api/tables/{id}/occupy
```

#### Response `204 No Content`

#### Regras

- A mesa precisa existir.
- A mesa não pode estar removida.
- A mesa não pode já estar ocupada.

---

### 8.6 Liberar mesa manualmente

```http
PATCH /api/tables/{id}/release
```

#### Response `204 No Content`

#### Regras

- A mesa precisa existir.
- A mesa não pode estar removida.
- A mesa precisa estar ocupada.

---

### 8.7 Remover mesa

```http
DELETE /api/tables/{id}
```

#### Response `204 No Content`

#### Regras

- A mesa precisa existir.
- Não é possível remover mesa ocupada.
- A remoção é feita via **soft delete**.

---

## 9. Pedidos

Pedido é o fluxo central da operação do restaurante.

Um pedido:

- pertence a uma mesa;
- possui um ou mais itens;
- possui status;
- calcula total com base nos itens;
- ao ser criado, ocupa a mesa;
- ao ser entregue ou cancelado, libera a mesa.

---

## 10. Status do pedido

Enum `OrderStatus`:

| Status | Valor lógico | Descrição |
|---|---:|---|
| `Created` | `1` | Pedido criado. |
| `InPreparation` | `2` | Pedido em preparo. |
| `Ready` | `3` | Pedido pronto. |
| `Delivered` | `4` | Pedido entregue. |
| `Canceled` | `5` | Pedido cancelado. |

No JSON, os status são enviados e recebidos como string.

Exemplo:

```json
{
  "status": "InPreparation"
}
```

---

## 11. Fluxo de status do pedido

Fluxo permitido:

```txt
Created -> InPreparation -> Ready -> Delivered
```

Cancelamento:

```txt
Created        -> Canceled
InPreparation  -> Canceled
Ready          -> Canceled
```

Restrições:

- Pedido `Delivered` não pode ser cancelado.
- Pedido `Delivered` não pode receber novos itens.
- Pedido `Canceled` não pode receber novos itens.
- Pedido só pode ir para `InPreparation` se estiver `Created`.
- Pedido só pode ir para `Ready` se estiver `InPreparation`.
- Pedido só pode ir para `Delivered` se estiver `Ready`.

---

## 12. Criar pedido

```http
POST /api/orders
```

### Request

```json
{
  "tableId": "f988d730-5457-451e-adab-47028cf2c097"
}
```

### Response `201 Created`

```json
{
  "id": "274bc5e2-33bd-4b3d-8815-e577697139e4"
}
```

### Regras

- A mesa precisa existir.
- A mesa não pode estar ocupada.
- O pedido começa com status `Created`.
- Ao criar o pedido, a mesa é marcada como ocupada.

---

## 13. Adicionar item ao pedido

```http
POST /api/orders/{id}/items
```

### Request

```json
{
  "dishId": "c878392b-e6a5-4824-a2bb-7142ae970737",
  "quantity": 1
}
```

### Response `204 No Content`

Sem corpo de resposta.

### Regras

- O pedido precisa existir.
- O prato precisa existir.
- O prato precisa estar disponível.
- A quantidade deve ser maior que zero.
- O item usa o preço atual do prato no momento da inclusão.
- Pedido `Delivered` não pode receber item.
- Pedido `Canceled` não pode receber item.

### Observação importante para o frontend

O total do pedido não é enviado no `POST /items`. Para atualizar a tela com o total recalculado, o frontend deve buscar o pedido novamente:

```http
GET /api/orders/{id}
```

---

## 14. Listar pedidos

```http
GET /api/orders
```

### Filtros

| Query param | Tipo | Descrição |
|---|---|---|
| `status` | `OrderStatus` | Filtra por status. Exemplo: `Created`, `Ready`, `Delivered`. |
| `tableId` | `Guid` | Filtra pedidos de uma mesa específica. |
| `startDate` | `DateTime` | Filtra por data inicial de criação do pedido. |
| `endDate` | `DateTime` | Filtra por data final de criação do pedido. |
| `pageNumber` | `int` | Página. |
| `pageSize` | `int` | Tamanho da página. |

### Exemplos

```http
GET /api/orders?pageNumber=1&pageSize=10
```

```http
GET /api/orders?status=Created&pageNumber=1&pageSize=10
```

```http
GET /api/orders?status=Delivered&startDate=2026-05-01&endDate=2026-05-31&pageNumber=1&pageSize=10
```

### Response `200 OK`

```json
{
  "items": [
    {
      "id": "274bc5e2-33bd-4b3d-8815-e577697139e4",
      "tableId": "f988d730-5457-451e-adab-47028cf2c097",
      "tableNumber": 2,
      "status": "Created",
      "total": 29.9,
      "items": [
        {
          "id": "7a7df96e-9b6a-4c9c-9872-679832bda4c4",
          "dishId": "c878392b-e6a5-4824-a2bb-7142ae970737",
          "dishName": "Smash Burger",
          "quantity": 1,
          "unitPrice": 29.9,
          "total": 29.9
        }
      ],
      "createdAt": "2026-05-23T11:38:20.3334217",
      "updatedAt": "2026-05-23T11:44:07.3214596",
      "deliveredAt": null
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

---

## 15. Buscar pedido por ID

```http
GET /api/orders/{id}
```

### Response `200 OK`

```json
{
  "id": "274bc5e2-33bd-4b3d-8815-e577697139e4",
  "tableId": "f988d730-5457-451e-adab-47028cf2c097",
  "tableNumber": 2,
  "status": "Created",
  "total": 29.9,
  "items": [
    {
      "id": "7a7df96e-9b6a-4c9c-9872-679832bda4c4",
      "dishId": "c878392b-e6a5-4824-a2bb-7142ae970737",
      "dishName": "Smash Burger",
      "quantity": 1,
      "unitPrice": 29.9,
      "total": 29.9
    }
  ],
  "createdAt": "2026-05-23T11:38:20.3334217",
  "updatedAt": "2026-05-23T11:44:07.3214596",
  "deliveredAt": null
}
```

---

## 16. Alterar status do pedido

```http
PATCH /api/orders/{id}/status
```

### Request

```json
{
  "status": "InPreparation"
}
```

### Response `204 No Content`

Sem corpo de resposta.

### Status aceitos nesse endpoint

- `InPreparation`
- `Ready`
- `Delivered`

Para cancelar, use o endpoint específico:

```http
PATCH /api/orders/{id}/cancel
```

---

## 17. Cancelar pedido

```http
PATCH /api/orders/{id}/cancel
```

### Response `204 No Content`

Sem corpo de resposta.

### Regras

- O pedido precisa existir.
- Pedido entregue não pode ser cancelado.
- Ao cancelar, a mesa é liberada se estiver ocupada.

---

## 18. Fluxo completo recomendado para o frontend

### 18.1 Preparar cardápio

1. Criar categoria.
2. Criar prato vinculado à categoria.
3. Listar pratos disponíveis para exibir no cardápio digital.

Endpoints:

```http
POST /api/categories
POST /api/dishes
GET  /api/dishes?isAvailable=true
```

---

### 18.2 Preparar mesas

1. Criar mesas.
2. Listar mesas livres.

Endpoints:

```http
POST /api/tables
GET  /api/tables?isOccupied=false
```

---

### 18.3 Abrir pedido

1. Usuário seleciona uma mesa livre.
2. Frontend chama `POST /api/orders`.
3. A API cria o pedido e ocupa a mesa.
4. Frontend guarda o `orderId`.

Endpoint:

```http
POST /api/orders
```

Request:

```json
{
  "tableId": "f988d730-5457-451e-adab-47028cf2c097"
}
```

---

### 18.4 Adicionar itens

1. Usuário seleciona prato e quantidade.
2. Frontend chama `POST /api/orders/{id}/items`.
3. Após `204`, frontend chama `GET /api/orders/{id}` para obter total atualizado.

Endpoints:

```http
POST /api/orders/{id}/items
GET  /api/orders/{id}
```

---

### 18.5 Cozinha acompanha pedidos

A tela da cozinha pode buscar pedidos em preparo ou criados:

```http
GET /api/orders?status=Created&pageNumber=1&pageSize=20
GET /api/orders?status=InPreparation&pageNumber=1&pageSize=20
```

Fluxo:

```txt
Created -> InPreparation -> Ready
```

Endpoints:

```http
PATCH /api/orders/{id}/status
```

Payload para iniciar preparo:

```json
{
  "status": "InPreparation"
}
```

Payload para marcar como pronto:

```json
{
  "status": "Ready"
}
```

---

### 18.6 Entregar pedido

Quando o pedido estiver pronto e for entregue:

```http
PATCH /api/orders/{id}/status
```

Payload:

```json
{
  "status": "Delivered"
}
```

Ao entregar:

- pedido fica `Delivered`;
- campo `deliveredAt` é preenchido;
- mesa é liberada;
- pedido passa a aparecer no histórico de vendas;
- pedido passa a entrar nos cálculos do dashboard.

---

### 18.7 Cancelar pedido

```http
PATCH /api/orders/{id}/cancel
```

Ao cancelar:

- pedido fica `Canceled`;
- mesa é liberada;
- pedido não entra no histórico de vendas;
- pedido não entra no dashboard.

---

## 19. Histórico de vendas

O histórico de vendas usa somente pedidos com status `Delivered`.

### 19.1 Listar histórico

```http
GET /api/sales-history
```

### 19.2 Filtros

| Query param | Tipo | Descrição |
|---|---|---|
| `startDate` | `DateTime` | Data inicial da venda. |
| `endDate` | `DateTime` | Data final da venda. |
| `pageNumber` | `int` | Página. |
| `pageSize` | `int` | Tamanho da página. |

### 19.3 Exemplo

```http
GET /api/sales-history?startDate=2026-05-01&endDate=2026-05-31&pageNumber=1&pageSize=10
```

### 19.4 Response `200 OK`

```json
{
  "items": [
    {
      "orderId": "274bc5e2-33bd-4b3d-8815-e577697139e4",
      "tableId": "f988d730-5457-451e-adab-47028cf2c097",
      "tableNumber": 2,
      "total": 59.8,
      "saleDate": "2026-05-23T12:01:00",
      "items": [
        {
          "dishId": "c878392b-e6a5-4824-a2bb-7142ae970737",
          "dishName": "Smash Burger",
          "quantity": 2,
          "unitPrice": 29.9,
          "total": 59.8
        }
      ]
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

---

## 20. Dashboard

O dashboard calcula indicadores usando apenas pedidos com status `Delivered`.

### 20.1 Buscar dashboard

```http
GET /api/dashboard
```

### 20.2 Filtros

| Query param | Tipo | Descrição |
|---|---|---|
| `startDate` | `DateTime` | Data inicial do período. |
| `endDate` | `DateTime` | Data final do período. |

### 20.3 Exemplo

```http
GET /api/dashboard?startDate=2026-05-01&endDate=2026-05-31
```

### 20.4 Response `200 OK`

```json
{
  "totalRevenue": 159.7,
  "totalOrders": 4,
  "totalItemsSold": 8,
  "averageTicket": 39.925,
  "topDishes": [
    {
      "dishId": "c878392b-e6a5-4824-a2bb-7142ae970737",
      "dishName": "Smash Burger",
      "quantitySold": 5,
      "revenue": 149.5
    }
  ]
}
```

### 20.5 Campos do dashboard

| Campo | Tipo | Descrição |
|---|---|---|
| `totalRevenue` | `decimal` | Faturamento total no período. |
| `totalOrders` | `int` | Quantidade de pedidos entregues. |
| `totalItemsSold` | `int` | Soma das quantidades vendidas. |
| `averageTicket` | `decimal` | Faturamento dividido pela quantidade de pedidos. |
| `topDishes` | `array` | Top 5 pratos mais vendidos. |

---

## 21. Recomendações para integração frontend

### 21.1 Sempre tratar erros de regra de negócio

Exemplos de mensagens que o frontend deve exibir ao usuário:

```txt
Não é possível criar pedido para uma mesa ocupada.
Não é possível adicionar um prato indisponível ao pedido.
Somente pedidos em preparo podem ficar prontos.
Pedidos entregues não podem ser cancelados.
```

---

### 21.2 Atualizar pedido após adicionar item

O endpoint de adicionar item retorna `204`, então o frontend deve buscar o pedido novamente para atualizar:

- lista de itens;
- total;
- updatedAt.

Fluxo recomendado:

```txt
POST /api/orders/{id}/items
GET  /api/orders/{id}
```

---

### 21.3 Usar paginação em todas as listagens

Evite chamadas sem paginação explícita em telas principais.

Recomendado:

```http
GET /api/dishes?pageNumber=1&pageSize=20
```

---

### 21.4 Usar filtros por status em telas operacionais

Tela de pedidos abertos:

```http
GET /api/orders?status=Created&pageNumber=1&pageSize=20
```

Tela da cozinha:

```http
GET /api/orders?status=InPreparation&pageNumber=1&pageSize=20
```

Tela de pedidos prontos:

```http
GET /api/orders?status=Ready&pageNumber=1&pageSize=20
```

Histórico:

```http
GET /api/orders?status=Delivered&pageNumber=1&pageSize=20
```

---

### 21.5 Não confiar no cálculo de total no frontend

O frontend pode calcular total para exibição temporária, mas o valor confiável deve vir da API:

```http
GET /api/orders/{id}
```

Motivo:

- o backend usa o preço do prato no momento da inclusão;
- regras de negócio ficam centralizadas no domínio;
- evita divergência entre app mobile, web e painel administrativo.

---

## 22. Ordem sugerida de integração no frontend

### Etapa 1 — Cardápio

- Listar categorias.
- Listar pratos por categoria.
- Criar/editar/remover pratos no painel administrativo.

Endpoints principais:

```http
GET  /api/categories
GET  /api/dishes
POST /api/categories
POST /api/dishes
PUT  /api/dishes/{id}
DELETE /api/dishes/{id}
```

---

### Etapa 2 — Mesas

- Listar mesas livres e ocupadas.
- Criar mesas no painel administrativo.
- Mostrar status visual da mesa.

Endpoints principais:

```http
GET  /api/tables
POST /api/tables
PATCH /api/tables/{id}/occupy
PATCH /api/tables/{id}/release
```

---

### Etapa 3 — Pedido

- Abrir pedido para mesa.
- Adicionar itens.
- Mostrar total.
- Alterar status.
- Cancelar pedido.

Endpoints principais:

```http
POST  /api/orders
POST  /api/orders/{id}/items
GET   /api/orders/{id}
PATCH /api/orders/{id}/status
PATCH /api/orders/{id}/cancel
```

---

### Etapa 4 — Operação da cozinha

- Listar pedidos por status.
- Atualizar status para preparo.
- Atualizar status para pronto.

Endpoints principais:

```http
GET   /api/orders?status=Created
GET   /api/orders?status=InPreparation
PATCH /api/orders/{id}/status
```

---

### Etapa 5 — Gestão

- Histórico de vendas.
- Dashboard.
- Filtros por período.

Endpoints principais:

```http
GET /api/sales-history
GET /api/dashboard
```

---

## 23. Exemplos rápidos de curl

### Criar categoria

```bash
curl -X POST "http://localhost:5280/api/categories" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Hambúrgueres",
    "description": "Categoria de hambúrgueres artesanais"
  }'
```

### Criar prato

```bash
curl -X POST "http://localhost:5280/api/dishes" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Smash Burger",
    "description": "Pão brioche, carne smash, cheddar e molho especial.",
    "price": 29.9,
    "categoryId": "6e90fa5f-88b1-48c0-aabd-4e3fc1bb9cc7"
  }'
```

### Criar mesa

```bash
curl -X POST "http://localhost:5280/api/tables" \
  -H "Content-Type: application/json" \
  -d '{
    "number": 2
  }'
```

### Criar pedido

```bash
curl -X POST "http://localhost:5280/api/orders" \
  -H "Content-Type: application/json" \
  -d '{
    "tableId": "f988d730-5457-451e-adab-47028cf2c097"
  }'
```

### Adicionar item ao pedido

```bash
curl -X POST "http://localhost:5280/api/orders/274bc5e2-33bd-4b3d-8815-e577697139e4/items" \
  -H "Content-Type: application/json" \
  -d '{
    "dishId": "c878392b-e6a5-4824-a2bb-7142ae970737",
    "quantity": 1
  }'
```

### Buscar pedido

```bash
curl -X GET "http://localhost:5280/api/orders/274bc5e2-33bd-4b3d-8815-e577697139e4"
```

### Alterar status para preparo

```bash
curl -X PATCH "http://localhost:5280/api/orders/274bc5e2-33bd-4b3d-8815-e577697139e4/status" \
  -H "Content-Type: application/json" \
  -d '{
    "status": "InPreparation"
  }'
```

### Alterar status para pronto

```bash
curl -X PATCH "http://localhost:5280/api/orders/274bc5e2-33bd-4b3d-8815-e577697139e4/status" \
  -H "Content-Type: application/json" \
  -d '{
    "status": "Ready"
  }'
```

### Entregar pedido

```bash
curl -X PATCH "http://localhost:5280/api/orders/274bc5e2-33bd-4b3d-8815-e577697139e4/status" \
  -H "Content-Type: application/json" \
  -d '{
    "status": "Delivered"
  }'
```

### Consultar dashboard

```bash
curl -X GET "http://localhost:5280/api/dashboard?startDate=2026-05-01&endDate=2026-05-31"
```

---

## 24. Resumo dos endpoints

### Categorias

```http
POST /api/categories
GET  /api/categories
```

### Pratos

```http
POST   /api/dishes
GET    /api/dishes
GET    /api/dishes/{id}
PUT    /api/dishes/{id}
DELETE /api/dishes/{id}
```

### Mesas

```http
POST   /api/tables
GET    /api/tables
GET    /api/tables/{id}
PUT    /api/tables/{id}
DELETE /api/tables/{id}
PATCH  /api/tables/{id}/occupy
PATCH  /api/tables/{id}/release
```

### Pedidos

```http
POST  /api/orders
POST  /api/orders/{id}/items
GET   /api/orders
GET   /api/orders/{id}
PATCH /api/orders/{id}/status
PATCH /api/orders/{id}/cancel
```

### Histórico de vendas

```http
GET /api/sales-history
```

### Dashboard

```http
GET /api/dashboard
```

---

## 25. Pontos importantes para evolução futura

Atualmente a API ainda não possui autenticação nem multi-tenancy. Como o produto é um SaaS, os próximos passos naturais serão:

- autenticação com JWT;
- cadastro de restaurantes;
- vínculo de usuários a restaurantes;
- isolamento dos dados por restaurante;
- permissões por perfil, como administrador, atendente e cozinha;
- auditoria de alterações;
- pagamentos e fechamento de conta;
- integração com impressora ou tela de cozinha;
- SignalR para atualização em tempo real dos pedidos.

