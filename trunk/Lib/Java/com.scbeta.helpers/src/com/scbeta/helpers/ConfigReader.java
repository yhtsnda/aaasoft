/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.io.*;
import java.util.*;

/**
 *
 * @author aaa
 */
// 配置文件读取类
public class ConfigReader {
    // 配置文件名称(包括路径)

    String configFileName;
    // 保存配置的键值对
    Map<String, String> htConfigs;
    // 检测字符串编码类实例
    SinoDetect encodingDetecter = new SinoDetect();

    // 构造函数
    public ConfigReader(String configFileName) {
        this.configFileName = configFileName;
    }

    // 读取配置到字典中
    public boolean ReadConfig() {
        htConfigs = new LinkedHashMap<String, String>();
        // 得到BufferedReader
        BufferedReader reader = IoHelper.GetBufferedReaderFromFile(configFileName);

        do {
            String line;
            try {
                // 从文件中读取一行
                line = reader.readLine();
            } catch (IOException e) {
                e.printStackTrace();
                return false;
            }
            // 如果已经到文件尾，则退出
            if (line == null) {
                break;
            }
            // 如果是注释行，则不处理
            if (line.startsWith(";") || line.startsWith("#") || line.startsWith("=")) {
                continue;
            }
            // 如果不包括等号
            if (!line.contains("=")) {
                continue;
            }

            String[] tmpStrs = line.split("=");
            String key = tmpStrs[0].trim();
            String value = tmpStrs[1].trim();
            if (!htConfigs.containsKey(key)) {
                htConfigs.put(key, value);
            }
        } while (true);

        // 关闭配置文件
        try {
            reader.close();
        } catch (IOException e) {
            e.printStackTrace();
        }

        return true;
    }

    // 获取配置(返回String)
    public String GetString(String key) {
        if (!htConfigs.containsKey(key)) {
            ConsoleHelper.println(String.format("警告：未找到名称为%s的配置。", key));
        }
        return htConfigs.get(key);
    }

    // 获取配置(返回int)
    public int GetInt(String key) {
        try {
            String value = htConfigs.get(key);
            int i;
            i = Integer.parseInt(value);
            return i;
        } catch (Exception e) {
            return -1;
        }
    }
    
    // 获取配置(返回boolean)
    public boolean GetBoolean(String key){
        try {
            String value = htConfigs.get(key);
            return Boolean.parseBoolean(value);
        } catch (Exception e) {
            return false;
        }
    }
}