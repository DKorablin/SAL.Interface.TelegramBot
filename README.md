# Telegram SAL interface
[![Auto build](https://github.com/DKorablin/SAL.Interface.TelegramBot/actions/workflows/release.yml/badge.svg)](https://github.com/DKorablin/SAL.Interface.TelegramBot/releases/latest)

Interfaces and base contracts for building modular Telegram bot plugins within the SAL (Service Abstraction Layer) architecture. This library standardizes interaction patterns (message ingestion, command routing, session/context handling, and outbound operations) so plugins remain lightweight, testable, and decoupled from the underlying Telegram transport details. It enables:
- Consistent abstractions for updates, users, chats, and message pipelines
- Plugin-oriented extension points (commands, handlers, middlewares)
- Clear separation between domain logic and Telegram API specifics
- Easier unit testing via mockable interfaces
- Uniform integration in larger SAL-based solutions targeting .NET Framework 4.8+ and .NET Standard 2.0+

Use it as the foundation layer to implement reusable bot features without duplicating low-level Telegram handling code.