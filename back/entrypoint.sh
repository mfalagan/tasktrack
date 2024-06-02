#!/bin/bash
set -e

# Wait for the database to be ready
echo "Waiting for MySQL to be ready..."
while ! nc -z db 3306; do
  sleep 1
done

# Apply the migrations
echo "Applying EF Core migrations..."
dotnet ef database update --no-build --project /app/back.csproj

# Run the application
echo "Starting the application..."
exec dotnet back.dll
