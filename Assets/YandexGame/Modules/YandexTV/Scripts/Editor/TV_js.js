
let lastBackPressTime = 0;
let previousButtonState = {}; 

function TVInit() {
    console.log('Init TV ysdk: isTV =', ysdk.deviceInfo.isTV());
    if (ysdk.deviceInfo.isTV()) {
        ysdk.onEvent(ysdk.EVENTS.HISTORY_BACK, () => {
            const currentTime = Date.now();

            if (!initGame || currentTime - lastBackPressTime < 300) {
                ysdk.dispatchEvent(ysdk.EVENTS.EXIT);
            } else {
                lastBackPressTime = currentTime;
                myGameInstance.SendMessage('YandexGame', 'TVKeyBack');
            }
        });

        document.addEventListener("keydown", (e) => {
            myGameInstance.SendMessage('YandexGame', 'TVKeyDown', e.key);
        });

        document.addEventListener("keyup", (e) => {
            myGameInstance.SendMessage('YandexGame', 'TVKeyUp', e.key);
        });

        function UpdateGamepadState() {
            var gamepads = navigator.getGamepads();
            for (var i = 0; i < gamepads.length; i++) {
                var gamepad = gamepads[i];
                if (gamepad) {
                    if (gamepad.buttons[12].pressed && !previousButtonState[12]) {
                        myGameInstance.SendMessage('YandexGame', 'TVKeyDown', 'Up');
                    } else if (!gamepad.buttons[12].pressed && previousButtonState[12]) {
                        myGameInstance.SendMessage('YandexGame', 'TVKeyUp', 'Up');
                    }
                    if (gamepad.buttons[13].pressed && !previousButtonState[13]) {
                        myGameInstance.SendMessage('YandexGame', 'TVKeyDown', 'Down');
                    } else if (!gamepad.buttons[13].pressed && previousButtonState[13]) {
                        myGameInstance.SendMessage('YandexGame', 'TVKeyUp', 'Down');
                    }
                    if (gamepad.buttons[14].pressed && !previousButtonState[14]) {
                        myGameInstance.SendMessage('YandexGame', 'TVKeyDown', 'Left');
                    } else if (!gamepad.buttons[14].pressed && previousButtonState[14]) {
                        myGameInstance.SendMessage('YandexGame', 'TVKeyUp', 'Left');
                    }
                    if (gamepad.buttons[15].pressed && !previousButtonState[15]) {
                        myGameInstance.SendMessage('YandexGame', 'TVKeyDown', 'Right');
                    } else if (!gamepad.buttons[15].pressed && previousButtonState[15]) {
                        myGameInstance.SendMessage('YandexGame', 'TVKeyUp', 'Right');
                    }

                    previousButtonState[12] = gamepad.buttons[12].pressed;
                    previousButtonState[13] = gamepad.buttons[13].pressed;
                    previousButtonState[14] = gamepad.buttons[14].pressed;
                    previousButtonState[15] = gamepad.buttons[15].pressed;
                }
            }
        }

        function GameLoop() {
            UpdateGamepadState();
            requestAnimationFrame(GameLoop);
        }

        GameLoop();
    }
}