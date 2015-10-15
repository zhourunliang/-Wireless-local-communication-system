//#include<reg52.h>
#include<C:\Program Files (x86)\Keil\C51\INC\STC\STC89C5xRC.H>
#include <intrins.h>

#define EEPROM_START_ADDRESS 0x2000//EEPROM��ʼ��ַ����
#define NOP() _nop_()//nopָ�һ������1���������ڣ���ʱ�ⲿ����Ϊ12MHZ���ʻ�������Ϊ1΢��

typedef unsigned char UINT8;
typedef unsigned int UINT16;
typedef unsigned long UINT32;





UINT8 flag=0,a, b;
UINT8 receive[4]={0,0,0,0};//���ջ���



/*********΢�뼶��ʱ*****************************/
void Delay_US(UINT16 us)
{
do{
NOP();
}while(--us > 0);	
}

/*********���뼶��ʱ*****************************/
void Delay_MS(UINT16 ms)
{
for(;ms>0;ms--){
Delay_US(1000);
}
}

/*********ʹ��EEPROM*****************************/
void EEPROMEnable()
{
ISP_CONTR = 0x81;//ʹ��EEPROM�������õȴ�ʱ�䡣
}

/*********����EEPROM******************************/
void EEPROMStart()
{
ISP_TRIG = 0x46;//�Ⱥ�д�����������ݣ��Դ���EEPROM����
ISP_TRIG = 0xB9;
}

/********����EEPROM*******************************/
void EEPROMDisable()
{
ISP_DATA = 0x00;//���ú󣬶����ݴ洢�ռ����㣬������ɸ���
ISP_ADDRH = 0x00;
ISP_ADDRL = 0x00;//��ַλ����
ISP_CONTR = 0x00;
}

/**********����������*****************************/
void EEPROMSectorErase(UINT16 addr)
{
EEPROMEnable();	
//	EA =0;
ISP_CMD = 0x03;//���������������
ISP_ADDRH = (UINT8)( (addr + EEPROM_START_ADDRESS)>>8 );
ISP_ADDRL = (UINT8) addr;
EEPROMStart();
Delay_MS(10);//�����ȴ�ʱ�䣬Լ10����
Delay_US(900);//��һ10������ʱ�ȴ�����������900΢�����ʱ�ȴ�ʱ�䣬ȷ������������ɡ�
EEPROMDisable();
//	EA = 1;
}


/**********д��һ���ֽ�****************************/
void EEPROMWriteByte(UINT16 addr,UINT8 byte)
{
EEPROMEnable();	
//	EA =0;
ISP_CMD = 0x02;//дģʽ
ISP_ADDRH = (UINT8)( (addr + EEPROM_START_ADDRESS)>>8 );
ISP_ADDRL = (UINT8) addr;
ISP_DATA = byte;//д�����ݣ���һ���ֽڣ�
EEPROMStart();
Delay_US(60);//д�ֽڵĵȴ�ʱ��
EEPROMDisable();
//	EA = 1;
}

/**********��ȡ�ֽ�*********************************/
UINT8 EEPROMReadByte(UINT16 addr)
{
EEPROMEnable();	
//	EA =0;
ISP_CMD = 0x01;//��ģʽ
ISP_ADDRH = (UINT8)( (addr + EEPROM_START_ADDRESS)>>8 );
ISP_ADDRL = (UINT8) addr;	
EEPROMStart();	
Delay_US(11);	
return (ISP_DATA);
//	EA = 1;
}

/*******��ʱ����*************/
void delay(unsigned int i)
{
    unsigned char j;
    for(i; i > 0; i--)    //ѭ��600*255�� ����������ִ����Ҫһ��ʱ�� Ҳ�ʹﵽ����ʱЧ��
        for(j = 255; j > 0; j--);
}

 /*******�м̺���*************/

void ser() interrupt 4
{
   
	static unsigned char count=0;	//���ڽ��ռ����ı���
	RI=0;			//�Ȱѽ��ձ�־λ����
	 b=EEPROMReadByte(0);
	receive[count]=SBUF;

   	if(receive[count]==0xaa) 					    	//����λ	��������λΪ0xaa
	{
			count=1;
	}
    else if(receive[count]==0xbb)						 //д��ַλ	  ��������λΪ0xbb
	{
		    count=1;									
	}
	else if(receive[count]==0xdd)						 //д��ַλ	  ��������λΪ0xbb
	{
		    count=1;									
	}
	else if(receive[0]==0xaa&&receive[count]==b)		//��ʶλ�����ֽ�
	{
		count=2;
	}
	else if(count==2)
	{
		count++;
	}
	else if(count==3&&receive[count]==~receive[2])   //У��λΪ����λȡ��
	{
		flag=1;
		count=0;
		
	}
	
	else if(count==1)		    
	{
		  	
		 if(receive[0]==0xbb)						 //д��ַλ	  ��������λΪ0xbb
		{
			flag=2;
			count=0;
		}
		else if(receive[0]==0xdd&&receive[1]==b)						   // ��ѯ			  ��������λΪ0xdd
		{
			flag=4;		
			count=0;	
		}
		else
		{
			count=0;
		}

	}
	
	
	
	else if(receive[count]==0xcc)						// ����ַλ		 ��������λΪ0xcc
	{
		    flag=3;		
			count=0;						
	}

	else
	{
		count=0;
	}

}

 /*******������*************/

void main()
{
	TMOD=0x20;	 //���ö�ʱ��1Ϊ������ʽ2
	TH1=0xfd;
	TL1=0xfd;	 //����������Ϊ9600
	TR1=1;		 //������ʱ��1
	REN=1;		 //�����������λ��
	SM0=0;
	SM1=1;		 //SM0SM1=01,��Ӧ���ڷ�ʽ1�������ʿɱ䡣
	EA=1;		 //�����ж�
	ES=1;		 //�򿪴����ж�
	
	while(1)
	{
		if(flag==1)							//����λ
			{
				ES=0;
				flag=0;
				P0=receive[2];
				SBUF=P0; //�ɹ��������ݣ����ؿ���λ
				P1=0xfe;  //P1.0��
				delay(600); // ������ʱ����  �ȴ�һ��ʱ���Ϩ�� 
				P1=0xff; 	//��P1��Ϊ�ߵ�ƽ Ϩ��P1��8��LED��
  				delay(600); // ������ʱ����
				while(!TI);		//TI�����жϱ�־λ������ָʾһ֡��Ϣ�Ƿ�����ɡ�
				TI=0;
				ES=1;

			}
		else if(flag==2)				  // д�ŵ�ַ��
			{   
				ES=0;
				flag=0;
				b=receive[1];
				EEPROMSectorErase(0);		  // ������1��������2000h~21FFh��
	            EEPROMWriteByte(0,b);	  // ��EEPROM��2002hд��b
				SBUF=EEPROMReadByte(0);
				P1=0xfd;  //P1.1��
				delay(600); // ������ʱ����  �ȴ�һ��ʱ���Ϩ�� 
				P1=0xff; 	//��P1��Ϊ�ߵ�ƽ Ϩ��P1��8��LED��
  				delay(600); // ������ʱ����
				while(!TI);		//TI�����жϱ�־λ������ָʾһ֡��Ϣ�Ƿ�����ɡ�
				TI=0;
				ES=1;
			}
		else if(flag==3)					//����ַ��
		    {
				ES=0;
				flag=0;
				SBUF=EEPROMReadByte(0);
				P1=0xfd;  //P1.1��
				delay(600); // ������ʱ����  �ȴ�һ��ʱ���Ϩ�� 
				P1=0xff; 	//��P1��Ϊ�ߵ�ƽ Ϩ��P1��8��LED��
  				delay(600); // ������ʱ����
				while(!TI);		//TI�����жϱ�־λ������ָʾһ֡��Ϣ�Ƿ�����ɡ�
				TI=0;
				ES=1;	
			}
		 else if(flag==4)					//��ѯ״̬
		    {
				ES=0;
				flag=0;
				SBUF=P0;
				P1=0xfe;  //P1.0��
				delay(600); // ������ʱ����  �ȴ�һ��ʱ���Ϩ�� 
				P1=0xff; 	//��P1��Ϊ�ߵ�ƽ Ϩ��P1��8��LED��
  				delay(600); // ������ʱ����
				while(!TI);		//TI�����жϱ�־λ������ָʾһ֡��Ϣ�Ƿ�����ɡ�
				TI=0;
				ES=1;	
			} 
	}
}

