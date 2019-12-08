/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

namespace StormRend.Units
{
    /// <summary>
    /// The possible results when a unit is pushed
    /// </summary>
    public enum PushResult
    {
        Nothing,    //Nothing hit
        HitUnit,       //Unit hit
        HitBlockedTile,     //Unwalkable tile hit
        OverEdge    //Pushed over the edge
    }
}