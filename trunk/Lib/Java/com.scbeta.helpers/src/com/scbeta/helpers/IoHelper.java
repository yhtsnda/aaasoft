/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.UnsupportedEncodingException;
import java.net.Socket;
import java.net.URLDecoder;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author aaa
 */
public class IoHelper {

    // 字符串编码类实例
    public static SinoDetect encodingDetecter = new SinoDetect();

    // 删除文件
    public static void DeleteFile(String FileName) {
        File file = new File(FileName);
        if (file.exists()) {
            file.delete();
        }
    }

    // 重命名文件
    public static void RenameFile(String srcFileName, String destFileName) {
        File file = new File(srcFileName);
        if (file.exists()) {
            file.renameTo(new File(destFileName));
        }
    }

    // 获取启动jar文件路径
    public static String GetStartupFilePath(Class clazz) {
        String tmpPath = clazz.getProtectionDomain().getCodeSource().getLocation().getPath();
        try {
            tmpPath = URLDecoder.decode(tmpPath, "UTF-8");
        } catch (UnsupportedEncodingException ex) {
            Logger.getLogger(IoHelper.class.getName()).log(Level.SEVERE, null, ex);
        }
        File f = new File(tmpPath);
        tmpPath = f.getPath();
        return tmpPath;
    }

    // 获取启动路径
    public static String GetStartupFolderPath(Class clazz) {
        String JarFileFullName = GetStartupFilePath(clazz);

        String JarFilePath = "";
        if (JarFileFullName.endsWith(".jar")) {
            File f = new File(JarFileFullName);
            JarFilePath = f.getParent();
        } else {
            JarFilePath = JarFileFullName;
        }

        if (JarFilePath == null) {
            JarFilePath = "";
        } else {
            JarFilePath += System.getProperty("file.separator");
        }
        return JarFilePath;
    }

    // 得到文本文件的BufferedReader(自动选择正确的字符编码)
    public static BufferedReader GetBufferedReaderFromFile(String filename) {
        FileInputStream fis;
        String encodingName;
        try {
            File file = new File(filename);
            // 获取编码
            int encodeIndex = encodingDetecter.detectEncoding(file);
            if (encodeIndex == SinoDetect.OTHER) {
                encodeIndex = SinoDetect.UTF8;
            }
            encodingName = SinoDetect.nicename[encodeIndex];
            fis = new FileInputStream(filename);
        } catch (FileNotFoundException e) {
            e.printStackTrace();
            return null;
        }

        InputStreamReader isr;
        try {
            isr = new InputStreamReader(fis, encodingName);
        } catch (UnsupportedEncodingException e1) {
            e1.printStackTrace();
            return null;
        }

        BufferedReader reader = new BufferedReader(isr);
        return reader;
    }

    // 将文件写入到Socket中
    public static boolean WriteFileToSocket(String filename, Socket socket) {
        try {
            FileInputStream fis = new FileInputStream(filename);
            OutputStream os = socket.getOutputStream();

            int bufferSize = 1024;
            byte[] buffer = new byte[bufferSize];
            while (true) {
                int len = fis.read(buffer);
                if (len <= 0) {
                    break;
                }
                os.write(buffer, 0, len);
            }
            fis.close();
        } catch (Exception e) {
            return false;
        }
        return true;
    }

    //将Socket的数据写入到文件中
    public static boolean WriteFileFromSocket(String filename, Socket socket) {
        try {
            FileOutputStream fos = new FileOutputStream(filename);
            InputStream os = socket.getInputStream();

            int bufferSize = 1024;
            byte[] buffer = new byte[bufferSize];
            while (true) {
                int len = os.read(buffer);
                if (len <= 0) {
                    break;
                }
                fos.write(buffer, 0, len);
            }
            fos.close();
        } catch (Exception e) {
            return false;
        }
        return true;
    }

    public static void CopyStream(InputStream input, OutputStream output, long totalLength) throws IOException {
        int bufferSize = 4096;
        CopyStream(input, output, bufferSize, totalLength);
    }

    public static void CopyStream(InputStream input, OutputStream output, int bufferSize, long totalLength) throws IOException {
        long position = 0;
        byte[] buffer = new byte[bufferSize];
        do {
            int CurrentReadSize = bufferSize;
            if ((totalLength - position) < bufferSize) {
                CurrentReadSize = (int) (totalLength - position);
            }

            int readCount = input.read(buffer, 0, CurrentReadSize);
            output.write(buffer, 0, readCount);

            position += readCount;
        } while (position < totalLength);
    }
}
