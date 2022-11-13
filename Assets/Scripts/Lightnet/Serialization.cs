using System;
using System.Collections.Generic;
using UnityEngine;

public static class SerializationHelper
{
    public static float FromBytes(byte[] data, ref int offset)
    {
        Debug.Assert(offset + sizeof(float) <= data.Length);
        float value = BitConverter.ToSingle(data, offset);
        offset += sizeof(float);
        return value;
    }

    // public static bool FromBytes(byte[] data, ref int offset, ref bool boolValue)
    // {
    //     Debug.Assert(offset + sizeof(bool) <= data.Length);
    //     float value = BitConverter.ToSingle(data, offset);
    //     offset += sizeof(bool);
    //     return Convert.ToBoolean(value);
    // }

    public static void FromBytes(byte[] data, ref int offset, ref float floatValue)
    {
        floatValue = FromBytes(data, ref offset);
    }

    public static void FromBytes(byte[] data, ref int offset, ref bool boolValue)
    {
        boolValue = Convert.ToBoolean(FromBytes(data, ref offset));
    }

    public static void FromBytes(byte[] data, ref int offset, ref Vector3 vector)
    {
        vector.x = FromBytes(data, ref offset);
        vector.y = FromBytes(data, ref offset);
        vector.z = FromBytes(data, ref offset);
    }
    
    // public static void FromBytes(byte[] data, ref int offset, ref List<float> list)
    // {
    //     // for (int i = 0; i < list.Count; ++i)
    //     // {
    //     //     list[i] = FromBytes(data,  ref offset);
    //     // }
    //     list[0] = FromBytes(data,  ref offset);
    //     list[1] = FromBytes(data,  ref offset);
    //     list[2] = FromBytes(data,  ref offset);
    //     list[3] = FromBytes(data,  ref offset);
    //     list[4] = FromBytes(data,  ref offset);
    //     list[5] = FromBytes(data,  ref offset);
    //     list[6] = FromBytes(data,  ref offset);
    //     list[7] = FromBytes(data,  ref offset);
    //     list[8] = FromBytes(data,  ref offset);
    //     list[9] = FromBytes(data,  ref offset);
    //     list[10] = FromBytes(data,  ref offset);
    //     list[11] = FromBytes(data,  ref offset);
    //     list[12] = FromBytes(data,  ref offset);
    //     list[13] = FromBytes(data,  ref offset);
    //     list[14] = FromBytes(data,  ref offset);
    //     list[15] = FromBytes(data,  ref offset);
    //     list[16] = FromBytes(data,  ref offset);
    //     list[17] = FromBytes(data,  ref offset);
    //     list[18] = FromBytes(data,  ref offset);
    //     list[19] = FromBytes(data,  ref offset);
    //     list[20] = FromBytes(data,  ref offset);
    //     list[21] = FromBytes(data,  ref offset);
    //     list[22] = FromBytes(data,  ref offset);
    //     list[23] = FromBytes(data,  ref offset);
    //     
    //
    // }

    public static void FromBytes(byte[] data, ref int offset, ref Quaternion quat)
    {
        quat.x = FromBytes(data, ref offset);
        quat.y = FromBytes(data, ref offset);
        quat.z = FromBytes(data, ref offset);
        quat.w = FromBytes(data, ref offset);
    }

    public static void ToBytes(ulong value, byte[] data, ref int offset)
    {
        Debug.Assert(offset + sizeof(float) < data.Length);
        byte[] buffer = BitConverter.GetBytes(value);
        Array.Copy(buffer, 0, data, offset, buffer.Length);
        offset += buffer.Length;
    }

    public static void ToBytes(float value, byte[] data, ref int offset)
    {
        Debug.Assert(offset + sizeof(float) <= data.Length);
        byte[] buffer = BitConverter.GetBytes(value);
        Array.Copy(buffer, 0, data, offset, buffer.Length);
        offset += buffer.Length;
    }
    
    public static void ToBytes(int value, byte[] data, ref int offset)
    {
        ToBytes((float)value, data, ref offset);
    }


    public static void ToBytes(bool value, byte[] data, ref int offset)
    {
        Debug.Assert(offset + sizeof(bool) <= data.Length);
        byte[] buffer = BitConverter.GetBytes(value);
        Array.Copy(buffer, 0, data, offset, buffer.Length);
        offset += buffer.Length;
    }

    public static void ToBytes(ref Vector3 vector, byte[] data, ref int offset)
    {
        ToBytes(vector.x, data, ref offset);
        ToBytes(vector.y, data, ref offset);
        ToBytes(vector.z, data, ref offset);
    }
    
    // public static void ToBytes(ref List<float> listy, byte[] data, ref int offset)
    // {
    //     // for (int i = 0; i < listy.Count; ++i)
    //     // {
    //     //     ToBytes(listy[i], data, ref offset);
    //     // }
    //     ToBytes(listy[0], data, ref offset);
    //     ToBytes(listy[1], data, ref offset);
    //     ToBytes(listy[2], data, ref offset);
    //     ToBytes(listy[3], data, ref offset);
    //     ToBytes(listy[4], data, ref offset);
    //     ToBytes(listy[5], data, ref offset);
    //     ToBytes(listy[6], data, ref offset);
    //     ToBytes(listy[7], data, ref offset);
    //     ToBytes(listy[8], data, ref offset);
    //     ToBytes(listy[9], data, ref offset);
    //     ToBytes(listy[10], data, ref offset);
    //     ToBytes(listy[11], data, ref offset);
    //     ToBytes(listy[12], data, ref offset);
    //     ToBytes(listy[13], data, ref offset);
    //     ToBytes(listy[14], data, ref offset);
    //     ToBytes(listy[15], data, ref offset);
    //     ToBytes(listy[16], data, ref offset);
    //     ToBytes(listy[17], data, ref offset);
    //     ToBytes(listy[18], data, ref offset);
    //     ToBytes(listy[19], data, ref offset);
    //     ToBytes(listy[20], data, ref offset);
    //     ToBytes(listy[21], data, ref offset);
    //     ToBytes(listy[22], data, ref offset);
    //     ToBytes(listy[23], data, ref offset);
    // }

    public static void ToBytes(ref Quaternion quat, byte[] data, ref int offset)
    {
        ToBytes(quat.x, data, ref offset);
        ToBytes(quat.y, data, ref offset);
        ToBytes(quat.z, data, ref offset);
        ToBytes(quat.w, data, ref offset);
    }
}

// This needs to be adjusted / extended to your needs
public enum ENetDataType : byte
{
    UserState = 1,
    ExperimentState = 2,
    SynchronizationState = 3
    // RandomState = 3 ,
    // ResponseState = 4
}


public interface NetworkData
{
    byte[] Serialize();
    void Deserialize(byte[] data);
}

public enum EExperimentStatus
{
    Waiting,  // Waiting for other participant
    WarmUp,   // Potential Countdown until something happens
    Running,  // Information is send and received , experiment runs
}

public class ExperimentState : NetworkData
{
    private const int SIZE =
        sizeof(byte) +  // ENetDataType.ExperimentState
        sizeof(byte) +  // socket Number 
        sizeof(byte);   // ExperimentState
    public byte SocketNumber;

    public EExperimentStatus Status;
    byte[] Cache = new byte[SIZE];
        
    public byte[] Serialize()
    {
        int offset = 0;
        Cache[offset] = (byte)ENetDataType.ExperimentState;     offset += sizeof(byte);
        Cache[offset] = SocketNumber;                           offset += sizeof(byte);
        Cache[offset] = (byte) Status;
        return Cache;
    }

    public void Deserialize(byte[] data)
    {
        int offset = 0;
        Debug.Assert(data[offset]== (byte) ENetDataType.ExperimentState);   offset += sizeof(byte);
        SocketNumber = data[offset];                                        offset += sizeof(byte);
        Status = (EExperimentStatus) data[offset];
    }
}

public class UserState : NetworkData
{
    private const int SIZE =
        // NOTE: using e.g. sizeof(Vector3) is not allowed...
        // so we need to always use sizeof(float) instead

        sizeof(byte) + // header     (ENetDataType.UserState)
        sizeof(float) * 3 + // Vector3    GazeSpherePosition;
        sizeof(float) * 3 + // Vector3    PlayerPosition
        sizeof(float) * 3 + // Vector3  camera pos
        sizeof(float) * 4 + // Vector3  camera rot
        sizeof(float) * 3 + // Vector3  rightHand pos
        sizeof(float) * 4 + // Vector3  rightHand rot
        sizeof(float) * 3 + // Vector3  leftHand pos
        sizeof(float) * 4 + // Vector3; leftHand rot 
        sizeof(float) * 4 + // Quaternion eyeShapePart1
        sizeof(float) * 4 + // Quaternion eyeShapePart2
        sizeof(float) * 4 + // Quaternion eyeShapePart3
        sizeof(float) * 4 + // Quaternion EyeBlinkVa1
        sizeof(float) * 4 + // Quaternion EyeBlinkVa2
        sizeof(float) * 3;     // float EyeBlinkVal3
        // sizeof(float) * 24 + // List EyeShapeTable 
        // sizeof(float) * 24 + // List EyeShape2IntTable 
        // sizeof(float) * 1 + // to float converted booleans   responseGiven, trialAnswer, playerReady
        // sizeof(float) * 1;  // bool     startExperiment 

    public Vector3 GazeSpherePosition;
    public Vector3 playerPosition;
    
    // player info
    public Vector3 VRCameraPosition;
    public Quaternion VRCameraRotation;
    public Vector3 VRRightHandPosition;
    public Quaternion VRRightHandRotation;
    public Vector3 VRLeftHandPosition;
    public Quaternion VRLeftHandRotation;

    public Quaternion eyeShapePart1; 
    public Quaternion eyeShapePart2;
    public Quaternion eyeShapePart3;
    
    public Quaternion EyeBlinkVal1; 
    public Quaternion EyeBlinkVal2;
    public Vector3 EyeBlinkVal3;
//
    // public List<float> EyeShapeTable;
    // public List<float> EyeShape2IntTable;

    // public bool playerReady;
    // public bool startExperiment;

    byte[] Cache = new byte[SIZE];


    public void Deserialize(byte[] data)
    {
        int head = 0;
        Debug.Assert(data.Length >= SIZE);
        Debug.Assert((ENetDataType) data[head] == ENetDataType.UserState);
        head += sizeof(byte);

        SerializationHelper.FromBytes(data, ref head, ref GazeSpherePosition);
        SerializationHelper.FromBytes(data, ref head, ref playerPosition);
        SerializationHelper.FromBytes(data, ref head, ref VRCameraPosition);
        SerializationHelper.FromBytes(data, ref head, ref VRCameraRotation);
        SerializationHelper.FromBytes(data, ref head, ref VRRightHandPosition);
        SerializationHelper.FromBytes(data, ref head, ref VRRightHandRotation);
        SerializationHelper.FromBytes(data, ref head, ref VRLeftHandPosition);
        SerializationHelper.FromBytes(data, ref head, ref VRLeftHandRotation);
        
        SerializationHelper.FromBytes(data, ref head, ref eyeShapePart1);
        SerializationHelper.FromBytes(data, ref head, ref eyeShapePart2);
        SerializationHelper.FromBytes(data, ref head, ref eyeShapePart3);
        
        SerializationHelper.FromBytes(data, ref head, ref EyeBlinkVal1);
        SerializationHelper.FromBytes(data, ref head, ref EyeBlinkVal2);
        SerializationHelper.FromBytes(data, ref head, ref EyeBlinkVal3);
        // SerializationHelper.FromBytes(data, ref head, ref EyeShapeTable);
        // SerializationHelper.FromBytes(data, ref head, ref EyeShape2IntTable);
        
        // SerializationHelper.FromBytes(data, ref head, ref playerReady);
        // SerializationHelper.FromBytes(data, ref head, ref startExperiment);
        //
    }

    public byte[] Serialize()
    {
        int head = 0;
        Cache[head] = (byte) ENetDataType.UserState;
        head += sizeof(byte);

        SerializationHelper.ToBytes(ref GazeSpherePosition, Cache, ref head);
        SerializationHelper.ToBytes(ref playerPosition, Cache, ref head);
        
        SerializationHelper.ToBytes(ref VRCameraPosition, Cache, ref head);
        SerializationHelper.ToBytes(ref VRCameraRotation, Cache, ref head);
        SerializationHelper.ToBytes(ref VRRightHandPosition, Cache, ref head);
        SerializationHelper.ToBytes(ref VRRightHandRotation, Cache, ref head);
        SerializationHelper.ToBytes(ref VRLeftHandPosition, Cache, ref head);
        SerializationHelper.ToBytes(ref VRLeftHandRotation, Cache, ref head);
        
        SerializationHelper.ToBytes(ref eyeShapePart1, Cache, ref head);
        SerializationHelper.ToBytes(ref eyeShapePart2, Cache, ref head);
        SerializationHelper.ToBytes(ref eyeShapePart3, Cache, ref head);

        SerializationHelper.ToBytes(ref EyeBlinkVal1, Cache, ref head);
        SerializationHelper.ToBytes(ref EyeBlinkVal2, Cache, ref head);
        SerializationHelper.ToBytes(ref EyeBlinkVal3, Cache, ref head);
        
        // SerializationHelper.ToBytes(ref EyeRightBlinkVal, Cache, ref head);
        // SerializationHelper.ToBytes(ref EyeShapeTable, Cache, ref head);
        // SerializationHelper.ToBytes(ref EyeShape2IntTable, Cache, ref head);
        
        // SerializationHelper.ToBytes(Convert.ToSingle(playerReady), Cache, ref head);
        // SerializationHelper.ToBytes(Convert.ToSingle(startExperiment), Cache, ref head);


        return Cache;
    }
}

public class SynchronizationState : NetworkData
{
    private const int SIZE =
        sizeof(byte) + // header     (ENetDataType.UserState)
        sizeof(float) * 4 + // size of all 4 synchro bools
        sizeof(float) * 6;  // size of all object vars

    public bool languageSelection;
    public bool enterExhibition;
    public bool exitExhibition;
    public bool newStartVisitor;

    public bool triggerOther1;
    public bool triggerOther2;
    public bool triggerOther3;
    public bool startOther1;
    public bool startOther2;
    public bool startOther3;
    
    byte[] Cache = new byte[SIZE];

    public void Deserialize(byte[] data)
    {
        int head = 0;
        Debug.Assert(data.Length >= SIZE);
        Debug.Assert((ENetDataType) data[head] == ENetDataType.SynchronizationState);
        head += sizeof(byte);
        
        SerializationHelper.FromBytes(data, ref head, ref languageSelection);
        SerializationHelper.FromBytes(data, ref head, ref enterExhibition);
        SerializationHelper.FromBytes(data, ref head, ref exitExhibition);
        SerializationHelper.FromBytes(data, ref head, ref newStartVisitor);
        SerializationHelper.FromBytes(data, ref head, ref triggerOther1);
        SerializationHelper.FromBytes(data, ref head, ref triggerOther2);
        SerializationHelper.FromBytes(data, ref head, ref triggerOther3);
        SerializationHelper.FromBytes(data, ref head, ref startOther1);
        SerializationHelper.FromBytes(data, ref head, ref startOther2);
        SerializationHelper.FromBytes(data, ref head, ref startOther3);
        
    }

    public byte[] Serialize()
    {
        int head = 0;
        Cache[head] = (byte) ENetDataType.SynchronizationState;
        head += sizeof(byte);
        
        SerializationHelper.ToBytes(Convert.ToSingle(languageSelection), Cache, ref head);
        SerializationHelper.ToBytes(Convert.ToSingle(enterExhibition), Cache, ref head);
        SerializationHelper.ToBytes(Convert.ToSingle(exitExhibition), Cache, ref head);
        SerializationHelper.ToBytes(Convert.ToSingle(newStartVisitor), Cache, ref head);
        SerializationHelper.ToBytes(Convert.ToSingle(triggerOther1), Cache, ref head);
        SerializationHelper.ToBytes(Convert.ToSingle(triggerOther2), Cache, ref head);
        SerializationHelper.ToBytes(Convert.ToSingle(triggerOther3), Cache, ref head);
        SerializationHelper.ToBytes(Convert.ToSingle(startOther1), Cache, ref head);
        SerializationHelper.ToBytes(Convert.ToSingle(startOther2), Cache, ref head);
        SerializationHelper.ToBytes(Convert.ToSingle(startOther3), Cache, ref head);
        return Cache;
    }
}
// public class RandomState : NetworkData
//     {
//         const int SIZE =
//             // NOTE: using e.g. sizeof(Vector3) is not allowed...
//             // so we need to always use sizeof(float) instead
//             
//             sizeof(byte) +     //header 
//             sizeof(float);     // random seed as float
//
//
//         public float randomSeed;
//  
//         byte[] Cache = new byte[SIZE];
//         
//         public byte[] Serialize()
//         {
//             int head = 0;
//             Cache[head] = (byte)ENetDataType.RandomState; 
//             head += sizeof(byte);
//             
//             SerializationHelper.ToBytes(randomSeed, Cache, ref head);
//
//             return Cache;
//         }
//
//         public void Deserialize(byte[] data)
//         {
//             int head = 0;
//             Debug.Assert(data.Length >= SIZE);
//             Debug.Assert((ENetDataType)data[head] == ENetDataType.RandomState);
//             head += sizeof(byte);
//
//             SerializationHelper.FromBytes(data, ref head, ref randomSeed);
//         }
//     }

// public class ResponseState : NetworkData
// {
//     const int SIZE =
//         // NOTE: using e.g. sizeof(Vector3) is not allowed...
//         // so we need to always use sizeof(float) instead
//             
//         sizeof(byte) +     //header 
//         sizeof(float) * 2;     // trialAnswer, responseGiven
//
//
//     public bool responseGiven;
//     public bool trialAnswer;
//  
//     byte[] Cache = new byte[SIZE];
//         
//     public byte[] Serialize()
//     {
//         int head = 0;
//         Cache[head] = (byte)ENetDataType.ResponseState; 
//         head += sizeof(byte);
//         
//         SerializationHelper.ToBytes(Convert.ToSingle(responseGiven), Cache, ref head);
//         SerializationHelper.ToBytes(Convert.ToSingle(trialAnswer), Cache, ref head);
//
//         return Cache;
//     }
//
//     public void Deserialize(byte[] data)
//     {
//         int head = 0;
//         Debug.Assert(data.Length >= SIZE);
//         Debug.Assert((ENetDataType)data[head] == ENetDataType.ResponseState);
//         head += sizeof(byte);
//
//         SerializationHelper.FromBytes(data, ref head, ref responseGiven);
//         SerializationHelper.FromBytes(data, ref head, ref trialAnswer);
//     }
//     
// }