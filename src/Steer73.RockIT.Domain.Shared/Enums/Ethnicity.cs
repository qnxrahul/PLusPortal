using System.ComponentModel;

namespace Steer73.RockIT.Enums;
public enum Ethnicity
{
    [Description("Asian or Asian British – any other Asian background")]
    AsianOrAsianBritishOther = 1,
    [Description("Asian or Asian British – Bangladeshi")] 
    AsianOrAsianBritishBangladeshi = 2,
    [Description("Asian or Asian British – Indian")] 
    AsianOrAsianBritishIndian = 3,
    [Description("Asian or Asian British – Pakistani")] 
    AsianOrAsianBritishPakistani = 4,
    [Description("Black or British – African")] 
    BlackOrBritishAfrican = 5,
    [Description("Black or British – any other Black background")] 
    BlackOrBritishOther = 6,
    [Description("Black or British – Caribbean")] 
    BlackOrBritishCaribbean = 7,
    [Description("Chinese")] 
    Chinese = 8,
    [Description("Mixed – any other mixed background")] 
    MixedOther = 9,
    [Description("Mixed – White and Asian")] 
    MixedWhiteAndAsian = 10,
    [Description("Mixed – White and Black African")] 
    MixedWhiteAndBlackAfrican = 11,
    [Description("Mixed – White and Black Caribbean")] 
    MixedWhiteAndBlackCaribbean = 12,
    [Description("White – any other White background")] 
    WhiteOther = 13,
    [Description("White – British")] 
    WhiteBritish = 14,
    [Description("White – Irish")]
    WhiteIrish = 15,
    [Description("Prefer not to say")] 
    PreferNotToSay = 16,
    [Description("Other")] 
    Other = 17
}

