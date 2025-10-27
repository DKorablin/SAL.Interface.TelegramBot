# Telegram SAL Interface
[![Auto build](https://github.com/DKorablin/SAL.Interface.TelegramBot/actions/workflows/release.yml/badge.svg)](https://github.com/DKorablin/SAL.Interface.TelegramBot/releases/latest)

Base interface library for building modular Telegram bot plugins within the SAL (Service Abstraction Layer) architecture.

## Overview

This library provides a standardized foundation for developing Telegram bot plugins using the SAL architecture. It abstracts the complexities of Telegram API interactions while maintaining a clean separation of concerns.

## Features

- **Standardized Abstractions**
  - Message handling and routing
  - User and chat context management
  - Command processing pipeline
  - Response formatting and delivery

- **Plugin Architecture Support**
  - Extensible command handlers
  - Custom middleware integration
  - Flexible message processors
  - Wizard-style conversation flows

## Target Frameworks

- .NET Framework 4.8
- .NET Standard 2.0

## Core Components

- **IChatProcessor**: Base interface for handling untyped Telegram messages
- **IChatUsage**: Interface for plugin usage documentation
- **ChatHandler**: Abstract base class for implementing chat handlers
- **WizardCtrl**: Support for multi-step conversation flows
- **ListCtrl**: Utilities for handling list-based interactions

## Implementation
- [TelegramBot plugin](https://github.com/DKorablin/Plugin.TelegramBot)

## Getting Started

1. Add the NuGet package to your project:
    ```powershell
    Install-Package SAL.Interface.TelegramBot
    ```

2. Implement chat handlers by inheriting from `ChatHandler`:
    ```csharp
    public class MyBotHandler : ChatHandler
    {
        public override IEnumerable<Reply> ProcessMessage()
        {
            // Your message handling logic
        }
    }
    ```

## Benefits

- **Maintainability**: Clear separation between domain logic and Telegram API specifics
- **Testability**: Easy unit testing through mockable interfaces
- **Reusability**: Portable plugin components across different bot implementations
- **Scalability**: Modular design supporting incremental feature additions

## Integration

Works seamlessly in larger SAL-based solutions, providing a consistent approach to bot feature implementation without duplicating Telegram-specific code.

## Requirements

- Visual Studio 2019+
- .NET Framework 4.8 or .NET Standard 2.0 compatible runtime