/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.io.IOException;
import java.net.Socket;
import java.util.LinkedList;
import java.util.List;

/**
 *
 * @author aaa
 */
public class ConsoleHelper {

    //是否自定义输出
    public static boolean isCustomOutput = false;
    private static List<Outputable> outputerList;

    //添加一个输出器
    public static void addOutputer(Outputable value) {
        if (outputerList == null) {
            outputerList = new LinkedList<Outputable>();
        }
        outputerList.add(value);
    }

    //可输出接口
    public interface Outputable {

        //输出
        public void print(String text);
    }

    // 输出到控制台文本
    public static void print(String text) {
        if (text != null) {
            if (!isCustomOutput) {
                System.out.print(text);
            } else {
                System.out.print(text);
                for (Outputable outputer : outputerList) {
                    outputer.print(text);
                }
            }
        }
    }

    // 输出到控制台文本
    public static void println(String text) {
        print(text + "\r\n");
    }

    // 输出到控制台文本（带时间）
    public static void printlogln(String text) {
        printlogln(text, null);
    }

    private static void printlogln(String text, Socket socket) {
        String nowTimeString = DateTimeHelper.GetNowDateTime();
        text = String.format("[%s]  %s", new Object[]{nowTimeString, text});
        println(text);
    }

    // 从控制台获取输入的一行，用来获取命令输入
    public static String readln() {
        String command = "";
        byte[] buffer = new byte[1024];
        try {
            int count = 0;
            do {
                int ch;
                ch = System.in.read();
                if (ch == 13) {
                    continue;
                }
                if (ch == 10) {
                    break;
                }
                buffer[count] = (byte) ch;
                count += 1;
            } while (true);
            command = new String(buffer, 0, count);
        } catch (IOException e) {
            e.printStackTrace();
        }
        command = command.trim();
        return command;
    }
}
