# Pirate Island
### Unity 2022.3.12f1
![Gameplay](Gameplay.gif)

<hr>

### Welcome to Pirate Island, an exciting game where you defend the island against invading pirates from your ship.

## Gameplay

- Shoot the appearing pirates on the island from your ship.
- Control the direction of your shots by moving the mouse cursor and clicking the left mouse button or tapping on the screen.
- The game ends when 10 pirates appear on the island.
- Pirate spawn rate increases over time.
- When a power-up appears, shoot it to activate.
- There are 2 kinds of power-ups:
  1. <span style = "color: #2A73C1">Blue</span> - speeds up your cannon shots.
  2. <span style="color: #C1452A">Red</span> - destroys all pirates on the island.

## Todo Improvements

- Player statistics are currently saved in PlayerPrefs for simplicity. For larger datasets, it is better to switch to JSON serialization.
- Refraining from comparing prefabs in ObjectPooler is needed. This change will allow you to configure ObjectPoolSettings as Addressables.
- I consider moving enemy and power-up data to ScriptableObjects for better organization.

## How to Play

1. Check the [Releases](https://github.com/srggrigorov/pirate-island/releases) tab and choose one for your platform;
2. Download the installation .exe file fow Windows, run it and follow instructions;
3. Download the .apk file for Android, move it to your phone folder and run it.

Feel free to contribute, report issues, or suggest enhancements. Have fun playing Pirate Island!

<hr>

### Добро пожаловать в игру "Pirate Island" — захватывающую игру, в которой вам предстоит защитить остров от нападения пиратов с вашего корабля.

## Геймплей

- Стреляйте по появляющимся пиратам на острове с вашего корабля.
- Управляйте направлением выстрелов, перемещая указатель мыши и кликая левой кнопкой мыши или нажимая по экрану.
- Игра заканчивается, когда на острове появится 10 пиратов.
- С течением времени увеличивается скорость появления пиратов.
- Когда появляется усиление, стреляйте по нему, чтобы активировать.
- Есть два вида усилений:
  1. <span style="color: #2A73C1">Синее</span> - ускоряет выстрелы из вашей пушки.
  2. <span style="color: #C1452A">Красное</span> - уничтожает всех пиратов на острове.

## Планы на улучшения

- Статистика игрока в настоящее время сохраняется в PlayerPrefs для упрощения. Для более крупных данных следует перейти к сериализации в формате JSON.
- Стоит избегать сравнения префабов в ObjectPooler. Это позволит настроить ObjectPoolSettings как Addressables.
- Я Рассматриваю возможность перемещения данных противников и усилений в ScriptableObjects для лучшей организации.

## Как играть

1. Проверьте вкладку [Releases](https://github.com/srggrigorov/pirate-island/releases) и выберите подходящий для вашей платформы;
2. Загрузите установочный файл .exe для Windows, запустите его и следуйте инструкциям;
3. Загрузите файл .apk для Android, переместите его в папку на телефоне и запустите.

Не стесняйтесь сообщать об ошибках или предлагать улучшения. Приятной игры!


