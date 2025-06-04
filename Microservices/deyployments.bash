#!/bin/bash

set -e

echo "🚀 Старт деплоя в namespace microservices..."

# 1. Создать namespace (если не существует)
kubectl apply -f namespace.yaml

# 2. Загрузить секреты
kubectl apply -f secrets.yaml
kubectl apply -f rabbitmq-deployment.yaml

# 3. Деплой ApiGateway
kubectl apply -f apigateway-deployment.yaml

# 4. Деплой ControllerFirst (AuthService)
kubectl apply -f controllerfirst-deployment.yaml

# 5. Деплой WeatherApp
kubectl apply -f weatherapp-deployment.yaml

echo "✅ Деплой завершен успешно!"
