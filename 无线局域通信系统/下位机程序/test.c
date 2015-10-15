//#include<reg52.h>
#include<C:\Program Files (x86)\Keil\C51\INC\STC\STC89C5xRC.H>
#include <intrins.h>

#define EEPROM_START_ADDRESS 0x2000//EEPROM起始地址设置
#define NOP() _nop_()//nop指令，一条消耗1个机器周期，此时外部晶振为12MHZ，故机器周期为1微秒

typedef unsigned char UINT8;
typedef unsigned int UINT16;
typedef unsigned long UINT32;





UINT8 flag=0,a, b;
UINT8 receive[4]={0,0,0,0};//接收缓存



/*********微秒级延时*****************************/
void Delay_US(UINT16 us)
{
do{
NOP();
}while(--us > 0);	
}

/*********毫秒级延时*****************************/
void Delay_MS(UINT16 ms)
{
for(;ms>0;ms--){
Delay_US(1000);
}
}

/*********使能EEPROM*****************************/
void EEPROMEnable()
{
ISP_CONTR = 0x81;//使能EEPROM，并设置等待时间。
}

/*********启动EEPROM******************************/
void EEPROMStart()
{
ISP_TRIG = 0x46;//先后写入这两个数据，以触发EEPROM启动
ISP_TRIG = 0xB9;
}

/********禁用EEPROM*******************************/
void EEPROMDisable()
{
ISP_DATA = 0x00;//禁用后，对数据存储空间清零，以免造成干扰
ISP_ADDRH = 0x00;
ISP_ADDRL = 0x00;//地址位清零
ISP_CONTR = 0x00;
}

/**********擦除扇形区*****************************/
void EEPROMSectorErase(UINT16 addr)
{
EEPROMEnable();	
//	EA =0;
ISP_CMD = 0x03;//触发命令，擦除扇区
ISP_ADDRH = (UINT8)( (addr + EEPROM_START_ADDRESS)>>8 );
ISP_ADDRL = (UINT8) addr;
EEPROMStart();
Delay_MS(10);//擦除等待时间，约10毫秒
Delay_US(900);//万一10毫秒延时等待不够，增加900微秒的延时等待时间，确保扇区擦除完成。
EEPROMDisable();
//	EA = 1;
}


/**********写入一个字节****************************/
void EEPROMWriteByte(UINT16 addr,UINT8 byte)
{
EEPROMEnable();	
//	EA =0;
ISP_CMD = 0x02;//写模式
ISP_ADDRH = (UINT8)( (addr + EEPROM_START_ADDRESS)>>8 );
ISP_ADDRL = (UINT8) addr;
ISP_DATA = byte;//写入数据，（一个字节）
EEPROMStart();
Delay_US(60);//写字节的等待时间
EEPROMDisable();
//	EA = 1;
}

/**********读取字节*********************************/
UINT8 EEPROMReadByte(UINT16 addr)
{
EEPROMEnable();	
//	EA =0;
ISP_CMD = 0x01;//读模式
ISP_ADDRH = (UINT8)( (addr + EEPROM_START_ADDRESS)>>8 );
ISP_ADDRL = (UINT8) addr;	
EEPROMStart();	
Delay_US(11);	
return (ISP_DATA);
//	EA = 1;
}

/*******延时函数*************/
void delay(unsigned int i)
{
    unsigned char j;
    for(i; i > 0; i--)    //循环600*255次 机器在这里执行需要一段时间 也就达到了延时效果
        for(j = 255; j > 0; j--);
}

 /*******中继函数*************/

void ser() interrupt 4
{
   
	static unsigned char count=0;	//串口接收计数的变量
	RI=0;			//先把接收标志位清零
	 b=EEPROMReadByte(0);
	receive[count]=SBUF;

   	if(receive[count]==0xaa) 					    	//控制位	当输入首位为0xaa
	{
			count=1;
	}
    else if(receive[count]==0xbb)						 //写地址位	  当输入首位为0xbb
	{
		    count=1;									
	}
	else if(receive[count]==0xdd)						 //写地址位	  当输入首位为0xbb
	{
		    count=1;									
	}
	else if(receive[0]==0xaa&&receive[count]==b)		//标识位两个字节
	{
		count=2;
	}
	else if(count==2)
	{
		count++;
	}
	else if(count==3&&receive[count]==~receive[2])   //校验位为控制位取反
	{
		flag=1;
		count=0;
		
	}
	
	else if(count==1)		    
	{
		  	
		 if(receive[0]==0xbb)						 //写地址位	  当输入首位为0xbb
		{
			flag=2;
			count=0;
		}
		else if(receive[0]==0xdd&&receive[1]==b)						   // 查询			  当输入首位为0xdd
		{
			flag=4;		
			count=0;	
		}
		else
		{
			count=0;
		}

	}
	
	
	
	else if(receive[count]==0xcc)						// 读地址位		 当输入首位为0xcc
	{
		    flag=3;		
			count=0;						
	}

	else
	{
		count=0;
	}

}

 /*******主程序*************/

void main()
{
	TMOD=0x20;	 //设置定时器1为工作方式2
	TH1=0xfd;
	TL1=0xfd;	 //将波特率设为9600
	TR1=1;		 //启动定时器1
	REN=1;		 //接收允许控制位。
	SM0=0;
	SM1=1;		 //SM0SM1=01,对应串口方式1，波特率可变。
	EA=1;		 //打开总中断
	ES=1;		 //打开串口中断
	
	while(1)
	{
		if(flag==1)							//控制位
			{
				ES=0;
				flag=0;
				P0=receive[2];
				SBUF=P0; //成功接收数据，返回控制位
				P1=0xfe;  //P1.0亮
				delay(600); // 调用延时程序  等待一段时间后熄灭 
				P1=0xff; 	//置P1口为高电平 熄灭P1口8个LED灯
  				delay(600); // 调用延时程序
				while(!TI);		//TI发送中断标志位，用于指示一帧信息是否发送完成。
				TI=0;
				ES=1;

			}
		else if(flag==2)				  // 写放地址码
			{   
				ES=0;
				flag=0;
				b=receive[1];
				EEPROMSectorErase(0);		  // 擦除第1个扇区（2000h~21FFh）
	            EEPROMWriteByte(0,b);	  // 对EEPROM区2002h写入b
				SBUF=EEPROMReadByte(0);
				P1=0xfd;  //P1.1亮
				delay(600); // 调用延时程序  等待一段时间后熄灭 
				P1=0xff; 	//置P1口为高电平 熄灭P1口8个LED灯
  				delay(600); // 调用延时程序
				while(!TI);		//TI发送中断标志位，用于指示一帧信息是否发送完成。
				TI=0;
				ES=1;
			}
		else if(flag==3)					//读地址码
		    {
				ES=0;
				flag=0;
				SBUF=EEPROMReadByte(0);
				P1=0xfd;  //P1.1亮
				delay(600); // 调用延时程序  等待一段时间后熄灭 
				P1=0xff; 	//置P1口为高电平 熄灭P1口8个LED灯
  				delay(600); // 调用延时程序
				while(!TI);		//TI发送中断标志位，用于指示一帧信息是否发送完成。
				TI=0;
				ES=1;	
			}
		 else if(flag==4)					//查询状态
		    {
				ES=0;
				flag=0;
				SBUF=P0;
				P1=0xfe;  //P1.0亮
				delay(600); // 调用延时程序  等待一段时间后熄灭 
				P1=0xff; 	//置P1口为高电平 熄灭P1口8个LED灯
  				delay(600); // 调用延时程序
				while(!TI);		//TI发送中断标志位，用于指示一帧信息是否发送完成。
				TI=0;
				ES=1;	
			} 
	}
}

