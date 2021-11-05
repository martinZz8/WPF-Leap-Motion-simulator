using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.Models
{
    enum InputTypes
    {
        NO_INPUT,
        RECEIVE_SMS_CODE,
        RECEIVE_PHONE_NUMBER,
        SEND_SENDER_FIRST_NAME,
        SEND_SENDER_LAST_NAME,
        SEND_SENDER_EMAIL,
        SEND_SENDER_PHONE_NUMBER,
        SEND_RECEIVER_FIRST_NAME,
        SEND_RECEIVER_LAST_NAME,
        SEND_RECEIVER_PHONE_NUMBER,
        SEND_RECEIVER_CITY,
        SEND_RECEIVER_POST_CODE,
        SEND_RECEIVER_STREET,
        SEND_RECEIVER_HOUSE_NUMBER,
        SEND_RECEIVER_APARTMENT_NUMBER,
        SEND_RECEIVER_HOUSE_LETTER
    }
}
