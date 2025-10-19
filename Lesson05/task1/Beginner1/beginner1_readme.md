# ConfigParser

## Опис
`ConfigParser` — простий парсер рядків конфігурації у форматі `ключ=значення`.
Програма зчитує рядок, перевіряє його на коректність та повертає ключ і значення у вигляді кортежу `(Key, Value)`.

---

## Як запустити
1. Відкрити проєкт у Visual Studio або будь-якому IDE для C#.
2. Запустити програму (`Ctrl+F5` або `Run`), вона обробить приклад:
```csharp
var setting = ConfigParser.ParseSetting("username=admin");
Console.WriteLine($"Key: {setting.Key}, Value: {setting.Value}");
```

**Приклад виводу:**
```
Key: username, Value: admin
```

---

## Особливості
- Порожні рядки або `null` викликають `ArgumentNullException`.
- Рядки без `=` викликають `FormatException`.
- Використовуються `nameof` для точних повідомлень про помилки.
- Підтримується обрізання пробілів навколо ключа та значення.

---

## Юніт-тести / тестові випадки
1. Коректний рядок: `"username=admin"` → повертає `("username", "admin")`.
2. Порожній рядок: `""` → кидає `ArgumentNullException`.
3. Рядок без `=`: `"username"` → кидає `FormatException`.
4. Рядок з пробілами: `"  user  =  123  "` → повертає `("user", "123")`.

