//综合枚举

public enum EAttachmentTableType
{
    ArticleContent = 1,//ArticleContent表附件
    ArticlePicture = 2,//ArticleContent表附件
    HospitalIllFj = 3,
    DischargeSummaryPic = 4
}

/// <summary>
/// 导出格式
/// </summary>
public enum EExportFormat
{
    /// <summary>
    /// CSV
    /// </summary>
    Csv,
    /// <summary>
    /// XLS
    /// </summary>
    Xls,
    /// <summary>
    /// DOC
    /// </summary>
    Doc,
    /// <summary>
    /// TXT
    /// </summary>
    Txt
}

/// <summary>
/// 系统固定账户
/// </summary>
public enum EAccount
{
    /// <summary>
    /// 不分配权限，系统自动分权限，拥有最高权限，一般为开发管理员
    /// </summary>
    SuperAdmin = 1,   //超级管理员
    /// <summary>
    /// 不分配权限，系统自动分权限，一般为开发人员
    /// </summary>
    SystemAdmin = 2,  //系统管理员
    /// <summary>
    /// 可分配权限，客户方最高级管理员，客服方不能删除、修改其权限内容
    /// </summary>
    Admin = 3,        //管理员
    /// <summary>
    /// 不分配权限，可进入无内容主页或其他无需权限可看内容
    /// </summary>
    Everyman = 4      //普通人员
}

/// <summary>
/// 账号类型
/// </summary>
public enum EAccountType
{
    None = 0,               //无
    SuperManager = 1,       //超级管理员
    SystemManager = 2,      //系统管理员
    GeneralManager = 3,     //管理员
    GeneralUser = 4,        //普通人员
    Visitor = 5             //游客
}

/// <summary>
/// 用户类型
/// </summary>
public enum EUserType
{
    System = 1,//系统用户
    Doctor = 2,//医生
    Nurse = 3,//护士
    NursesAide = 4,//普通护工
    NursesAideBzz = 5,//护工班组长
    Xz = 6,//行政
    Rs = 7,//人事
    Food = 8, //食堂工作人员
    Door = 9,//门卫
    HouQin = 10,//后勤
    SYiS = 10, //扫一扫
    YuanZhang = 11, //院长
    Customer = 12 //老人

}
/// <summary>
/// 数据同步--数据操作类型
/// </summary>
public enum ESyncUpDataType
{
    Add = 1,//新增
    Update = 2,//修改和逻辑删除
    Delete = 3,//物理删除
}
/// <summary>
/// 用户类型
/// </summary>
public enum ECustomerType
{
    Normal = 1,//正常数据
    Delete = -1,//删除
    Rzsqb = 11,//入住申请表
    Rzdfb = 12,//入住调访表
    Rzspb = 13,//入住审批表
    RuYuan = 14,//入院
}
/// <summary>
/// 消息类型
/// </summary>
public enum EMessageType
{
    Tzgg = 1,//通知公告
    Hlxx = 2,//护理消息
    Wcxx = 3//外出信息
}
/// <summary>
/// 字典类型
/// </summary>
public enum EDictionaryType
{
    Kinship = 1,//亲属关系
    NursingType = 2,//护理消息
    DrugUnit = 3,//药品单位（片，克）
    DrugManageUnit = 5,//药品单位(盒，瓶)
    Wedlock = 4,//婚姻关系
    YinShiType = 6,//饮食类型
    EducationType = 8,//老人文化程度
    OldManType = 9,//老人类型
    StreetType = 10,//老人类型
    CaseHistory = 11,// 病史类型
    DrugCLType = 12,// 药品材料分类
    KinshipByDoor = 15,// 亲属关系（门卫）
    FoodTYpe = 16,// 食物分类
    FloorArea = 20,// 楼层区域：防火巡查
    FloorName = 25,// 楼层区域:楼面餐具消毒
    MeTrAcRecordType = 26,   //会议、培训、活动记录 类型
    WeekRecipeType = 27  //食谱导出模本名称
}
/// <summary>
/// 药品出入库和盘存 类型
/// </summary>
public enum EDrugFoundationType
{
    Drug = 1,//药品
    DrugCL = 2//药品材料
}
//老人类型
public enum ECustomerByDicType
{
    tuiXiu = 901,//离退休老人
    ziFei = 902,//自费老人
    guLao = 903,//城市社会孤老
    xiaoYuLiuShi = 904,//小于六十岁
}
//护理等级
public enum ECustomerHLMC
{
    huLi0 = 0,
    huLi1 = 2,
    huLi2 = 2,
    huLi3 = 3,
    huLi4 = 4,
    huLi5 = 5,
    huLi6 = 6
}
/// <summary>
/// 平板节点 分类
/// </summary>
public enum ENodePadType
{
    Doctor = 1,//医生
    Nurse = 2,//护士
    Food = 3,//食堂
    Door = 4,//门卫
    Xz = 5,//行政
    Customer = 12 //老人

}

/// <summary>
/// 执行结果
/// </summary>
public enum EResultStatus
{
    /// <summary>
    /// 执行成功
    /// </summary>
    Success = 1,   //执行成功
    /// <summary>
    /// 执行失败
    /// </summary>
    Failure = 0   //执行失败
}

/// <summary>
/// 数据来源
/// </summary>
public enum ESoucreType
{
    None = 0,       //
    BgWeb = 1,      //后台WEB系统
    FrontWeb = 2,   //前台WEB系统
    WsWeb = 3,      //WEBSERVICE服务
    Phone = 4       //手机系统    
}

/// <summary>
/// 系统数据状态
/// </summary>
public enum ESystemStatus
{
    /// <summary>
    /// 有效
    /// </summary>
    Valid = 1,            //有效
    /// <summary>
    /// 无效
    /// 可是删除，禁用等
    /// </summary>
    Invalid = 0,           //无效
    /// <summary>
    /// 删除
    /// </summary>
    Deleted = -1,          //删除
    /// <summary>
    /// 禁用
    /// </summary>
    Forbidden = 2          //禁用
}
/// <summary>
/// 系统数据状态
/// </summary>
public enum EDoctorAdviceStatus
{
    /// <summary>
    /// 新开医嘱
    /// </summary>
    one = 1,            //医生
    /// <summary>
    ///  护士执行后医嘱状态
    /// </summary>
    two = 2,           //护士操作后
    /// <summary>
    /// 医生强制结束 医嘱状态后
    /// </summary>
    three = 3,          //医生
    /// <summary>
    /// 医生强制结束 医嘱状态后  护士执行
    /// </summary>
    four = 4          //护士
}
/// <summary>
/// 按钮类型 对应数据表[Buttons]主键
/// </summary>
public enum EButtonType
{
    Add = 1,                 //新增
    Update = 2,              //修改
    Delete = 3,              //删除(逻辑)
    PhyDelete = 4,           //物理删除
    Look = 5,                //查看
    Save = 6,                //保存
    Submit = 7,              //提交
    Back = 8,                //返回
    Import = 9,              //导入
    Export = 10,             //导出Excel
    Print = 11,              //打印    
    Confirm = 12,            //确定
    Cancel = 13,             //取消     
    Edit = 14,               //编辑
    Detail = 15,             //详细  
    Agree = 16,              //同意
    NoAgree = 17,            //不同意
    Pass = 18,               //通过
    NoPass = 19,             //不通过   
    ResetPassword = 20,      //重置密码
    Enable = 21,             //启用
    Disable = 22,            //禁用
    SetTop = 23,             //置顶
    SetNoTop = 24,           //取消置顶

    SetNodeButton = 29,      //设置节点按钮
    Return = 30,             //退回  
    SetHgBed = 31,             //设置管理床位

    AuthRoleMenu = 50,       //角色菜单权限
    AuthUserMenu = 51,       //用户菜单权限
    AuthRoleNode = 52,       //角色节点权限
    AuthUserNode = 53,       //用户节点权限
    AuthRoleNodeButton = 54, //角色节点(页面)按钮权限
    AuthUserNodeButton = 55,  //用户节点(页面)按钮权限
    CustomerLeave = 40,         //老人离院
    ExportWord = 42,         //导出word
}

/// <summary>
/// 错误等级
/// </summary>
public enum EErrorRank
{
    /// <summary>
    /// 无错误
    /// </summary>
    None = 0,           //无错误
    /// <summary>
    /// 信息
    /// </summary>
    Message = 1,        //信息
    /// <summary>
    /// 告警
    /// </summary>
    Alarm = 2,          //告警 
    /// <summary>
    /// 错误
    /// </summary>
    Error = 3           //错误
}

#region 日志
/// <summary>
/// 日志级别
/// </summary>
public enum ELogRank
{
    None = 0,
    Message = 1,        //信息
    Alarm = 2,          //警告 
    Error = 3           //错误
}

/// <summary>
/// 日志源
/// </summary>
public enum ELogSource
{
    None = 0,
    Client = 1,   //客户端
    User = 2,     //用户 
    System = 3,   //系统
    History = 4   //历史记录
}

/// <summary>
/// 操作事件类型
/// </summary>
public enum EventType
{
    None = 0,
    Manage = 1,         //管理
    Login = 2           //登录
}

/// <summary>
/// 日志读取状态
/// </summary>
public enum ELogRead
{
    UnRead = 0,         //未读
    HasRead = 1         //已读
}

/// <summary>
/// 清理日志时间
/// </summary>
public enum ELogClearTime
{
    None = 0,           //不清理(0)
    OneYear = 1,        //一年(1)
    TwoYear = 2,        //两年(2)
    HalfYear = 3,       //半年(3)
    QuarterYear = 4,    //三个月(4)
    OneMonth = 5        //一个月(5)
}
#endregion

/// <summary>
/// SESSION键
/// </summary>
public enum ESessionKeys
{
    /// <summary>
    /// 登录用户ID
    /// </summary>
    UserId,
    /// <summary>
    /// 用户登录唯一ID
    /// </summary>
    UserUniqueId,
    /// <summary>
    /// 登录用户
    /// </summary>
    User,
    /// <summary>
    /// 验证码
    /// </summary>
    ValidateCode,
    /// <summary>
    /// 是否要检查验证码
    /// </summary>
    IsValidateCode,
    /// <summary>
    /// 主题
    /// </summary>
    Theme,
    /// <summary>
    /// 根路径
    /// </summary>
    BaseUrl
}

/// <summary>
/// 缓存键
/// </summary>
public enum ECacheKeys
{
    /// <summary>
    /// 系统类实例(后跟类名)
    /// </summary>
    ClassInstance,
    UsersImpl,
    RolesImpl,
    DepartmentImpl

}

/// <summary>
/// 操作状态
/// </summary>
public enum EParamState
{
    Add = 1,           //添加
    Update = 2,           //修改
    DeleteL = 3,           //逻辑删除
    DeleteW = 4,           //物理删除
    Enable = 5,           //启用
    Disable = 6,           //禁用
}
