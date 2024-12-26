# RentalCar.Category
Sistema de gestão de Categoria da loja de aluguer de carros.

# Language
1. C#

# Framework
1. .NET CORE 8.0

# Data Base
1. MySql

# Arquitectura
1. Arquitetura Limpa (Clean Architecture)

# Padrões
1. CQRS
2. Repository

# Container
1. Docker
2. docker-compose
3. Requirements: Docker instalado
4. Run container: docker-compose up -d
5. Down container: docker-compose down

# Testes
1. Unitario (Fluent Assertions)
2. Integração

# Messageria
1. RabbitMq

# CI/CD
1.  GitHub Actions

# Logs
1. Serilog

# Observabilidade
1. OpenTelemetry

# Monitoramento
1. Prometheus 
2. Grafana

# Tracing 
1. Jeager

# SonarQube
docker run --name sonarqube -p 9000:9000 sonarqube:community


# Migration
dotnet ef migrations add FirstMigration --project RentalCar.Categories.Infrastructure -o Persistence/Migrations -s RentalCar.Categories.Api
dotnet ef database update --project RentalCar.Categories.Infrastructure -s RentalCar.Categories.Api
