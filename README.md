# ApiCuentas

API REST para gestión de cuentas bancarias, construida con .NET 10 siguiendo Clean Architecture, con pipeline de CI/CD completo y despliegue en Kubernetes.

## Stack

- **.NET 10** — Clean Architecture (Domain / Application / Infrastructure / Api) con CQRS vía MediatR
- **PostgreSQL** + EF Core (migraciones automáticas al arrancar)
- **FluentValidation** para validación de comandos
- **Serilog + Seq** para logging estructurado centralizado
- **Docker** (multistage build)
- **Kubernetes** (Helm chart) con liveness/readiness probes
- **GitHub Actions** — CI/CD con self-hosted runner

## Correr localmente con Docker Compose

```bash
docker-compose up -d
```

Levanta la API + PostgreSQL. La API queda disponible en `http://localhost:8080`, con Scalar (docs interactivas) en `/scalar`.

## Endpoints principales

```
GET  /api/v1/cuentas        # Lista todas las cuentas
POST /api/v1/cuentas        # Crea una cuenta nueva
```

## Desplegar en Kubernetes (Minikube)

```bash
kubectl apply -f k8s/00-namespace.yaml
kubectl apply -f k8s
helm upgrade --install apicuentas ./helm/apicuentas --namespace accounts
```

## Health checks

- `GET /health/live` — confirma que el proceso está vivo
- `GET /health/ready` — valida además la conexión a PostgreSQL

## CI/CD

En cada push a `main`, `.github/workflows/ci.yml` corre automáticamente:
1. **CI** (GitHub-hosted): restore, build, valida el Helm chart, build y push de la imagen a Docker Hub.
2. **CD** (self-hosted runner): despliega PostgreSQL, Seq y la API en Minikube local vía Helm.

## Estructura

```
src/
  ApiCuentas.Domain/          # Entidades
  ApiCuentas.Application/     # Commands, Queries, Validators (CQRS)
  ApiCuentas.Infrastructure/  # EF Core, Repositorios
  ApiCuentas.Api/             # Controllers, Program.cs
helm/apicuentas/               # Helm chart
k8s/                            # Manifiestos de Postgres y Seq
```
