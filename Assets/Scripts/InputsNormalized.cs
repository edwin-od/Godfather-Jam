using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsNormalized {

    public static int UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3;
    private bool isQwerty;
    public InputsNormalized() {
        isQwerty = Application.systemLanguage != SystemLanguage.French;
    }

    public InputsNormalized(bool isQwerty) {
        this.isQwerty = isQwerty;
    }

    public void setIsQwerty(bool isQwerty) {
        this.isQwerty = isQwerty;
    }
    public bool getIsQwerty() {
        return isQwerty;
    }

    public KeyCode realInput(bool p1, int input) {
        KeyCode key;
        switch(input) {
            case 0:     // UP
                if(p1) {
                    if(isQwerty)
                        key = KeyCode.W;
                    else
                        key = KeyCode.Z;
                } else
                    key = KeyCode.UpArrow;
                break;
            case 1:     // DOWN
                if(p1) {
                    if(isQwerty)
                        key = KeyCode.S;
                    else
                        key = KeyCode.S;
                } else
                    key = KeyCode.DownArrow;
                break;
            case 2:     // LEFT
                if(p1) {
                    if(isQwerty)
                        key = KeyCode.A;
                    else
                        key = KeyCode.Q;
                } else
                    key = KeyCode.LeftArrow;
                break;
            case 3:     // RIGHT
                if(p1) {
                    if(isQwerty)
                        key = KeyCode.D;
                    else
                        key = KeyCode.D;
                } else
                    key = KeyCode.RightArrow;
                break;
            default:
                key = KeyCode.None;
                break;
        }
        return key;
    }

}
