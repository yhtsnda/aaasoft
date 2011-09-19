/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.util.Random;

/**
 * 随机数辅助类
 * @author aaa
 */
public class RandomHelper {

    public static int GetRandomInt(int start, int end) {
        Random rnd = new Random();
        int randomValue = rnd.nextInt();
        randomValue = (randomValue % (end - start)) + start;        
        return randomValue;
    }
}
