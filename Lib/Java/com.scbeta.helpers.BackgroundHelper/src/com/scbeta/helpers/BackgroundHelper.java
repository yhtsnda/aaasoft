/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import com.scbeta.helpers.ConsoleHelper.Outputable;
import com.scbeta.net.xiep.XiepClient;
import com.scbeta.net.xiep.XiepClient.ServerDisconnectedListener;
import com.scbeta.net.xiep.XiepClient.ServerEventCameListener;
import com.scbeta.net.xiep.XiepServer;
import com.scbeta.net.xiep.XiepServer.ClientConnectedListener;
import com.scbeta.net.xiep.XiepServer.ClientDisconnectedListener;
import com.scbeta.net.xiep.XiepServer.ReceiveRequestListener;
import com.scbeta.net.xiep.eventArgs.ClientConnectedArgs;
import com.scbeta.net.xiep.eventArgs.ClientConnectionInfoArgs;
import com.scbeta.net.xiep.eventArgs.ReceiveRequestArgs;
import com.scbeta.net.xiep.eventArgs.XiepClientEventArgs;
import com.scbeta.net.xiep.packages.EventPackage;
import com.scbeta.net.xiep.packages.RequestPackage;
import com.scbeta.net.xiep.packages.ResponsePackage;
import java.io.File;
import java.io.IOException;
import java.net.Socket;
import java.util.Calendar;
import java.util.EventObject;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.Properties;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * 后台运行辅助类
 * @author aaa
 */
public class BackgroundHelper {

    private String programName;
    private Class mainClassClazz;
    private String foregroundProgramCommandPrompt = "VLI>";
    private String backgroundProgramCommandPrompt = "BACK VLI>";
    private int tcpPort;
    private XiepClient xiepClient;
    private XiepServer xiepServer;
    private List<Socket> connectedSocketList;
    private BackgroundWorkListener backgroundWorkListener;
    private String unknowenCommandMessage;
    private Map<String, CommandHandler> commandHandlerMap;
    private int waitMileseconds = 3000;
    // 监听端口是否空闲
    private boolean isListenPortFree = false;

    /**
     * 设置前台程序的命令提示符
     * @param value 
     */
    public void setForegroundProgramCommandPrompt(String value) {
        this.foregroundProgramCommandPrompt = value;
    }

    /**
     * 设置后台程序的命令提示符
     * @param value 
     */
    public void setBackgroundProgramCommandPrompt(String value) {
        this.backgroundProgramCommandPrompt = value;
    }

    /**
     * 设置等待时间
     * @param waitMileseconds 
     */
    public void setWaitMileseconds(int waitMileseconds) {
        this.waitMileseconds = waitMileseconds;
    }

    /**
     * 设置后台工作内容
     * @param value 
     */
    public void setBackgroundWorkListener(BackgroundWorkListener value) {
        this.backgroundWorkListener = value;
    }

    /**
     * 设置未知命令消息
     * @param unknowenCommandMessage 
     */
    public void setUnknowenCommandMessage(String unknowenCommandMessage) {
        this.unknowenCommandMessage = unknowenCommandMessage;
    }

    /**
     * 设置命令处理器映射Map
     * @param commandHandlerMap 
     */
    public void setCommandHandlerMap(Map<String, CommandHandler> commandHandlerMap) {
        this.commandHandlerMap = commandHandlerMap;
    }

    public interface BackgroundWorkListener {

        //执行后台任务
        public void execute(BackgroundHelper backgroundHelper, String[] args);
    }

    public interface CommandHandler {

        //执行命令并返回结果
        public String execute(String[] args);
    }

    //构造函数
    public BackgroundHelper(String programName, Class mainClassClazz, int tcpPort) {
        this.programName = programName;
        this.mainClassClazz = mainClassClazz;
        this.tcpPort = tcpPort;
        connectedSocketList = new LinkedList<Socket>();
    }

    // 测试网络，判断配置文件中的监听端口是否已开启监听
    private void testNetwork() {
        isListenPortFree = !NetHelper.testTcpPortListened("127.0.0.1", tcpPort);
    }

    public void start(String[] args) {
        testNetwork();
        boolean isStartABackgroundProgram = false;
        if (args != null && args.length > 0) {
            if (args[0].equals("-b")) {
                startBackground();
                backgroundWorkListener.execute(this, args);

                //等待命令输入
                while (true) {
                    System.out.print(backgroundProgramCommandPrompt);
                    String commandLine = ConsoleHelper.readln();
                    executeCommandLine(commandLine);
                }
            } else if (args[0].equals("-r")) {
                if (!startForeground()) {
                    ConsoleHelper.println("连接失败，程序退出！");
                    System.exit(0);
                }
            } else if (args[0].equals("-gc")) {
                startABacktroundProgram();
                if (!startForeground()) {
                    ConsoleHelper.println("连接失败，程序退出！");
                    System.exit(0);
                }
            } else {
                isStartABackgroundProgram = true;
            }
        } else {
            isStartABackgroundProgram = true;
        }

        if (isStartABackgroundProgram) {
            startABacktroundProgram();
            System.exit(0);
        }
    }

    private void executeCommandLine(String commandLine) {
        executeCommandLine(commandLine, null);
    }
    //执行命令行

    private void executeCommandLine(String commandLine, Socket socket) {

        //exit与stop属于基础命令
        //如果是退出
        if ("exit".equals(commandLine)) {
            System.exit(0);
            return;
        }

        //如果是停止
        if ("stop".equals(commandLine)) {
            //关闭所有的Socket
            for (Socket tmpSocket : connectedSocketList) {
                try {
                    tmpSocket.close();
                } catch (IOException ex) {
                }
            }
            System.exit(0);
            return;
        }

        //如果命令处理映射Map为null或为空
        if (this.commandHandlerMap == null || this.commandHandlerMap.isEmpty()) {
            return;
        }

        //命令参数
        String[] commandArgs = commandLine.split(" ");
        //命令
        String command = commandArgs[0];

        if (command == null || command.isEmpty()) {
        } else {
            //命令结果
            String commandResult = null;

            if (this.commandHandlerMap.containsKey(command)) {
                //执行命令
                commandResult = this.commandHandlerMap.get(command).execute(commandArgs);
            }

            if (commandResult == null && unknowenCommandMessage != null) {
                commandResult = unknowenCommandMessage;
            }

            //输出命令结果
            if (commandResult != null) {
                if (socket == null) {
                    ConsoleHelper.println(commandResult);
                } else {
                    this.sendServerOutput(commandResult, socket);
                }
            }
        }
    }

    private void startABacktroundProgram() {
        if (!isListenPortFree) {
            ConsoleHelper.println(programName + "已经启动，请使用 '-r' 参数连接。");
            System.exit(0);
        }
        ConsoleHelper.println("Starting background program...");

        String JarFilePath = IoHelper.GetStartupFilePath(mainClassClazz);
        File jarFile = new File(JarFilePath);
        if (!jarFile.exists()) {
            ConsoleHelper.println("未找到可执行Jar文件：" + JarFilePath);
            System.exit(0);
        }

        Properties props = System.getProperties(); // 获得系统属性集
        String osName = props.getProperty("os.name"); // 操作系统名称

        String exeCommand = String.format("java -jar %s -b", JarFilePath);

        try {
            Runtime run = Runtime.getRuntime();
            run.exec(exeCommand);
        } catch (Exception e) {
            ConsoleHelper.println(String.format("%s启动失败！\nStart program failed.Reason:%s", programName, e.getMessage()));
            System.exit(0);
        }

        long startWaitTime = Calendar.getInstance().getTimeInMillis();
        do {
            long nowTime = Calendar.getInstance().getTimeInMillis();
            //如果等待时间已经超过waitMileseconds
            if (nowTime - startWaitTime > this.waitMileseconds * 1000) {
                break;
            }

            testNetwork();
            try {
                Thread.sleep(100);
            } catch (InterruptedException ex) {
                Logger.getLogger(BackgroundHelper.class.getName()).log(Level.SEVERE, null, ex);
            }
        } while (isListenPortFree);

        if (isListenPortFree) {
            ConsoleHelper.println(String.format("%s启动失败，端口未监听！\nStart program failed,port not be listened.", programName));
        } else {
            ConsoleHelper.println(String.format("%s启动成功！\nStart program successfully!", programName));
        }
    }

    //启动后台
    private boolean startBackground() {
        try {
            xiepServer = new XiepServer(tcpPort);

            //自定义输出器
            ConsoleHelper.isCustomOutput = true;
            ConsoleHelper.addOutputer(new Outputable() {

                @Override
                public void print(String text) {
                    sendServerOutput(text);
                }
            });

            xiepServer.addClientConnectedListener(new ClientConnectedListener() {

                @Override
                public void Perform(ClientConnectedArgs e) {
                    ConsoleHelper.printlogln("有一个新的连接：" + e.getClientConnectionInfoArgs().toString());
                    synchronized (connectedSocketList) {
                        connectedSocketList.add(e.getClientConnectionInfoArgs().getSocket());
                    }

                }
            });
            xiepServer.addClientDisconnectedListener(new ClientDisconnectedListener() {

                @Override
                public void Perform(ClientConnectionInfoArgs e) {
                    ConsoleHelper.printlogln("连接已断开：" + e.toString());
                    synchronized (connectedSocketList) {
                        connectedSocketList.remove(e.getSocket());
                    }
                }
            });

            xiepServer.addReceiveRequestListener(new ReceiveRequestListener() {

                @Override
                public void Perform(ReceiveRequestArgs e) {
                    RequestPackage requestPackage = e.getRequestPackage();
                    if (requestPackage.getRequest().equals("Command")) {
                        String commandLine = requestPackage.getArgumentValue("CommandText");
                        executeCommandLine(commandLine, e.getClientConnectionInfoArgs().getSocket());
                    }
                    if (e.getResponsePackage() == null) {
                        e.setResponsePackage(new ResponsePackage(e.getRequestPackage(), "TODO"));
                    }
                }
            });
            xiepServer.start();
            return true;
        } catch (Exception ex) {
            return false;
        }
    }

    //启动前台
    private boolean startForeground() {
        // 判断程序是否已启动
        if (isListenPortFree) {
            ConsoleHelper.println(this.programName + "尚未启动，无法连接。");
            System.exit(0);
        }
        xiepClient = new XiepClient("127.0.0.1", tcpPort);
        xiepClient.addServerEventCameListener(new ServerEventCameListener() {

            @Override
            public void Perform(XiepClientEventArgs e) {
                EventPackage eventPackage = e.getEventPackage();
                if (eventPackage.getEvent().equals("ServerOutput")) {
                    String outputText = eventPackage.getArgumentValue("Output");
                    ConsoleHelper.println(outputText);
                }
            }
        });

        xiepClient.addServerDisconnectedListener(new ServerDisconnectedListener() {

            @Override
            public void Perform(EventObject e) {
                ConsoleHelper.printlogln("连接已经断开！");
                System.exit(0);
            }
        });

        long startTime = Calendar.getInstance().getTimeInMillis();
        do {
            try {
                xiepClient.start();
                break;
            } catch (Exception ex) {
            }

            xiepClient.stop();
            long nowTime = Calendar.getInstance().getTimeInMillis();
            if (nowTime - startTime > waitMileseconds * 1000) {
                return false;
            }
            try {
                Thread.sleep(100);
            } catch (InterruptedException ex) {
                Logger.getLogger(BackgroundHelper.class.getName()).log(Level.SEVERE, null, ex);
            }
        } while (true);

        while (true) {
            ConsoleHelper.print(foregroundProgramCommandPrompt);
            String commandLine = ConsoleHelper.readln();
            if (!sendCommand(commandLine)) {
                ConsoleHelper.println("未收到后台服务端返回的消息，程序退出!");
                System.exit(0);
                break;
            }
        }
        return true;
    }

    //发送服务器输出
    public void sendServerOutput(String outputText) {
        synchronized (connectedSocketList) {
            for (Socket socket : connectedSocketList) {
                sendServerOutput(outputText, socket);
            }
        }
    }

    //发送服务器输出
    public boolean sendServerOutput(String outputText, Socket socket) {
        EventPackage eventPackage = new EventPackage("ServerOutput");
        eventPackage.addArgument("Output", outputText);
        try {
            xiepServer.SendEvent(socket, eventPackage);
            return true;
        } catch (Exception ex) {
            return false;
        }
    }

    //发送命令
    public boolean sendCommand(String command) {
        if (command.equals("exit")) {
            System.exit(0);
        }

        RequestPackage requestPackage = new RequestPackage("Command");
        requestPackage.addArgument("CommandText", command);
        return xiepClient.SendRequest(requestPackage) != null;
    }
}
