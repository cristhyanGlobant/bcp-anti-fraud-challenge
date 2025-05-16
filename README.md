# 🛡️ Anti-Fraud Challenge

Este proyecto es una solución al reto técnico que consiste en desarrollar un sistema de detección de fraudes para transacciones financieras, basado en una arquitectura moderna de microservicios.

# 🧱 Arquitectura
Este proyecto sigue una **arquitectura basada en microservicios** con enfoque **hexagonal (puertos y adaptadores)**, buscando una alta separación de responsabilidades, facilidad de prueba, y bajo acoplamiento entre componentes. La solución está dividida en dos servicios principales:

## 📌 TransactionService
Responsable de exponer una API RESTful que permite crear transacciones. Al recibir una transacción:
* Persiste la transacción en una base de datos PostgreSQL.
* Publica un evento en Apache Kafka para su análisis antifraude.


## 📌 AntiFraudService
Este servicio consume los eventos desde Kafka. Se compone de:
* Un worker (servicio background) que escucha mensajes Kafka.
* Aplica una lógica de detección antifraude.
* Actualiza el estado de la transacción en la base de datos PostgreSQL mediante una API expuesta por el propio AntiFraudService.


## 🧩 Estructura Hexagonal
Cada servicio está dividido en capas siguiendo el patrón hexagonal:

```css
[Web API o Worker]
        ↓
[Application Layer] ←→ [Domain Layer]
        ↓
[Infrastructure Layer] ←→ [PostgreSQL / Kafka]
```


* `.Domain`: Contiene entidades de dominio y lógica de negocio.
* `.Application`: Contiene los casos de uso (orquestación).
* `.Infrastructure`: Adaptadores externos (repositorios, mensajería Kafka, persistencia).
* `.WebApi` o *`.Worker`: Adaptadores de entrada (API HTTP o procesos background).

## 🔌 Comunicación entre servicios
* Se usa Apache Kafka como canal de comunicación asincrónica entre servicios.


## 📦 Tecnologías y herramientas utilizadas

- [.NET 8](https://dotnet.microsoft.com/)
- [PostgreSQL](https://www.postgresql.org/)
- [Apache Kafka](https://kafka.apache.org/)
- [Docker](https://www.docker.com/)
- Arquitectura Hexagonal (Ports and Adapters)
- Pruebas unitarias con `xUnit` y `Moq`
- GitHub Actions (opcional para CI)



# 🛠️ Cómo ejecutar localmente

Requisitos:
- Docker y Docker Compose instalados

## 🔹 Paso 1: Clonar el repositorio

```bash
git clone https://github.com/cristhyanGlobant/bcp-anti-fraud-challenge.git
cd bcp-anti-fraud-challenge

docker-compose up --build
```

## 🔹 Paso 2: Levantar los servicios

```bash
docker-compose up --build
```

Esto levantará:
- PostgreSQL
- Kafka + Zookeeper
- TransactionService
- AntiFraudService

>Asegúrate de que los puertos `5432`, `29092`, `5001`, estén libres antes de iniciar Docker.

## 🔹 Paso 3: Enviar una transacción (ejemplo)

```bash
curl -X POST http://localhost:5001/api/transaction \
  -H "Content-Type: application/json" \
  -d '{
    "sourceAccountId": "11111111-1111-1111-1111-111111111111",
    "targetAccountId": "22222222-2222-2222-2222-222222222222",
    "transferTypeId": 1,
    "value": 150.75
}'
```

## 🔹 Paso 4: revisar los logs de cada servicio
Puedes revisar los logs de cada servicio con docker logs -f .

```bash
docker logs -f anti-fraud-challenge-transactionservice-1
docker logs -f anti-fraud-challenge-antifraudservice-1
```

## ✅ Pruebas

Para ejecutar las pruebas unitarias:

```bash
dotnet test
```

## 📂 Estructura del proyecto

```cs
bcp-anti-fraud-challenge/
├── docker-compose.yaml
├── services/
│ ├── TransactionService/
│ │ ├── TransactionService.sln
│ │ ├── src/
│ │ │ ├── TransactionService.Application/
│ │ │ ├── TransactionService.Domain/
│ │ │ ├── TransactionService.Infrastructure/
│ │ │ ├── TransactionService.WebApi/
│ │ └── tests/
│ │ └── UnitTests/
│ └── AntiFraudService/
│ ├── AntiFraudService.sln
│ ├── src/
│ │ ├── AntiFraudService.Application/
│ │ ├── AntiFraudService.Domain/
│ │ ├── AntiFraudService.Infrastructure/
│ │ ├── AntiFraudService.WebApi/
│ │ └── AntiFraudService.Worker/
│ └── tests/
│ └── UnitTests/
└── README.md
```


## ✍️ Autor
* Jorge Cristhyan Contreras
* GitHub
