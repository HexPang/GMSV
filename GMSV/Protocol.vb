Module Protocol
    Public Enum Protocol
#Region "基础包"
        PACK_HEAD = 12
        PACK_EOF = 22
#End Region
#Region "登录协议"
        LOGIN = 1000
        LOGOUT = 1010
#End Region
#Region "角色登录、创建部分"
        GET_CHARACTERS = 2000
        CREATE_CHARACTER = 2010
        LOGIN_CHARACTER = 2020
#End Region
        '发送行走协议时需在起步阶段发送当前坐标，并在停止移动时发送停止坐标
#Region "玩家状态协议"
        START_WALK = 3000 '玩家ID 方向 X Y 按键状态(按下，松开）
        STOP_WALK = 3100 '玩家ID 方向 X Y 按键状态(按下，松开）
        ATACKING = 3200 '玩家ID 方向 X Y 按键状态(按下，松开）
        HITING = 3300 '玩家ID [攻击源] 数值
        DEAD = 3400 '玩家ID
        JUMP = 3500 '跳
#End Region
#Region "玩家方向协议"
        LEFT = 6
        RIGHT = 2
#End Region
#Region "地图协议"
        MAP_ENTER = 4000 '玩家ID 昵称
        MAP_LEVEL = 4010 '玩家ID
#End Region
#Region "基本返回值"
        RET_SUCCESS = 5000
        RET_FAILED = 5010 ' 信息
        RET_ERROR = 5020 ' 信息
#End Region
    End Enum
End Module
