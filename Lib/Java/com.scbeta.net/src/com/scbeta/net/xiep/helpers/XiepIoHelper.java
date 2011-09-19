/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep.helpers;

import com.scbeta.helpers.ConsoleHelper;
import com.scbeta.helpers.IoHelper;
import com.scbeta.helpers.NumberHelper;
import com.scbeta.helpers.XmlTreeNode;
import com.scbeta.net.xiep.packages.AbstractXiepPackage;
import com.scbeta.net.xiep.packages.EventPackage;
import com.scbeta.net.xiep.packages.RequestPackage;
import com.scbeta.net.xiep.packages.ResponsePackage;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

/**
 * XIEP协议网络辅助类
 * @author aaa
 */
public class XiepIoHelper {

    //传输编码
    public static String transferEncoding = "utf-8";
    //最大接收包大小
    public static int maxReceivePackageSize = 10 * 1024 * 1024;

    // 发送XmlTreeNode包
    public static boolean SendPackage(Socket socket, AbstractXiepPackage xiepPackage) {
        synchronized (socket) {
            String responseNodeString = xiepPackage.toXml();
            try {
                OutputStream outputStream = socket.getOutputStream();

                byte[] buffer = responseNodeString.getBytes(transferEncoding);
                byte[] packageSizeByteArray = NumberHelper.intToByte(buffer.length);

                //写数据包大小
                outputStream.write(packageSizeByteArray);
                //写数据包内容
                outputStream.write(buffer);
                outputStream.flush();
                return true;
            } catch (Exception ex) {
                return false;
            }
        }
    }

    // 接收XML字符串
    public static String ReceiveXml(Socket socket) {
        InputStream inputStream = null;
        try {
            inputStream = socket.getInputStream();
        } catch (Exception ex) {
            return null;
        }
        synchronized (inputStream) {
            try {
                int packageSizeNumberBytesCount = 4;

                byte[] packageSizeByteArray = new byte[packageSizeNumberBytesCount];
                //读取包大小
                while (true) {
                    int readPackageSizeCount = inputStream.read(packageSizeByteArray, 0, packageSizeNumberBytesCount);
                    if (readPackageSizeCount < 0) {
                        throw new IOException(String.format("readPackageSizeCount为-1！", maxReceivePackageSize));
                    }
                    if (readPackageSizeCount != packageSizeNumberBytesCount) {
                        ConsoleHelper.println("readPackageSizeCount:" + readPackageSizeCount);
                        continue;
                    }
                    break;
                }

                int packageSize = NumberHelper.bytesToInt(packageSizeByteArray);

                if (packageSize <= 0) {
                    throw new IOException(String.format("包大小不能为负数，已丢弃！", maxReceivePackageSize));
                }
                if (packageSize > maxReceivePackageSize) {
                    throw new IOException(String.format("包大小超过 %s，已丢弃！", maxReceivePackageSize));
                }

                ByteArrayOutputStream outputStream = new ByteArrayOutputStream(packageSize);
                //读取数据
                IoHelper.CopyStream(inputStream, outputStream, packageSize);

                String xmlString = outputStream.toString(transferEncoding);
                //ConsoleHelper.println(String.format("包大小：%s，包内容：%s", packageSize,xmlString));
                return xmlString;
            } catch (IOException ex) {
                return null;
            }
        }
    }

    // 接收AbstractXiepPackage包
    public static AbstractXiepPackage ReceivePackage(Socket socket) {
        String xml = ReceiveXml(socket);
        if (xml == null || xml.isEmpty()) {
            return null;
        }

        AbstractXiepPackage rtnPackage = null;

        XmlTreeNode treeNode = XmlTreeNode.fromXml(xml);
        String packageName = treeNode.getKey();

        if (packageName.equals("RequestPackage")) {
            rtnPackage = new RequestPackage();
        } else if (packageName.equals("ResponsePackage")) {
            rtnPackage = new ResponsePackage();
        } else if (packageName.equals("EventPackage")) {
            rtnPackage = new EventPackage();
        } else {
            return null;
        }

        rtnPackage.setRootPackage(treeNode);
        return rtnPackage;
    }
}
