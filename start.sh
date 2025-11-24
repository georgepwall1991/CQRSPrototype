#!/bin/bash

# Simple start script for CQRSSolution

echo "🚀 Starting CQRSSolution..."

# Check for Docker
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker Desktop."
    exit 1
fi

# Check if Docker daemon is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker daemon is not running. Please start Docker Desktop."
    exit 1
fi

echo "🐳 Bringing up containers..."
docker-compose up -d --build

echo "✅ Solution started!"
echo "🌍 API: http://localhost:7001"
echo "📄 Swagger: http://localhost:7001/swagger"
echo "📝 Logs: docker-compose logs -f api"
