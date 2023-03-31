using Newtonsoft.Json;

namespace iFindCap
{
    public class iFinDData
    {
        public const ushort iFundPort = 8601;
        public const string Prefix = @"{""_id"":""";
        public const string Suffix = @"""}";
        public string _id { get; set; }
        public string area { get; set; }
        public string bid { get; set; }
        public string bidCbBp { get; set; }
        public double bidCbBpABS { get; set; }
        public string bidCsiBp { get; set; }
        public string bidHover { get; set; }
        public string bidId { get; set; }
        public string bidNetPrice { get; set; }
        public string bidQuoteStatus { get; set; }
        public string bidRemark { get; set; }
        public double bidSort { get; set; }
        public string bidTime { get; set; }
        public int bidType { get; set; }
        public string bidYield { get; set; }
        public string bondAbbreviation { get; set; }
        public string bondType { get; set; }
        public string bondTypeShow { get; set; }
        public string buyAmount { get; set; }
        public string buyAmountHover { get; set; }
        public string buyAmountSort { get; set; }
        public bool buyAmountSubscript { get; set; }
        public string cbBp { get; set; }
        public string cbDuration { get; set; }
        public string cbImpliedRating { get; set; }
        public string cbImpliedRatingsx { get; set; }
        public string cbNetPrice { get; set; }
        public string cbOfrBp { get; set; }
        public double cbOfrBpABS { get; set; }
        public string cbValuation { get; set; }
        public string chargeId { get; set; }
        public string conceptSort { get; set; }
        public string couponRate { get; set; }
        public string couponVariety { get; set; }
        public string crossMarket { get; set; }
        public string crossMarketShow { get; set; }
        public string csiBp { get; set; }
        public string csiCbBp { get; set; }
        public string csiImpliedRating { get; set; }
        public string csiImpliedRatingsx { get; set; }
        public string csiNetPrice { get; set; }
        public string csiOfrBp { get; set; }
        public string csiValuation { get; set; }
        public string dealPrice { get; set; }
        public string dealPriceType { get; set; }
        public string debtRating { get; set; }
        public string debtRatingFilter { get; set; }
        public string entitlementType { get; set; }
        public string expiryDate { get; set; }
        public string firstCode { get; set; }
        public string guaranteeAdult { get; set; }
        public string guaranteeThing { get; set; }
        public string guaranteeWay { get; set; }
        public string industry { get; set; }
        public string isCbHistory { get; set; }
        public string isCityInvestment { get; set; }
        public bool isLatest { get; set; }
        public string isNewList { get; set; }
        public string isPerpetual { get; set; }
        public string isValid { get; set; }
        public string market { get; set; }
        public string nextExerciseDate { get; set; }
        public string ofr { get; set; }
        public string ofrHover { get; set; }
        public string ofrId { get; set; }
        public string ofrNetPrice { get; set; }
        public string ofrRemark { get; set; }
        public double ofrSort { get; set; }
        public string ofrTime { get; set; }
        public int ofrType { get; set; }
        public string ofrYield { get; set; }
        public string orgDebtRating { get; set; }
        public string orgName { get; set; }
        public string orgNature { get; set; }
        public string orgRating { get; set; }
        public string orgRatingFilter { get; set; }
        public string outlook { get; set; }
        public string platform { get; set; }
        public string pledge { get; set; }
        public string ratingOrg { get; set; }
        public string remainingPeriod { get; set; }
        public int remainingPeriodSort { get; set; }
        public string remainingPeriodType { get; set; }
        public string secondaryCode { get; set; }
        public string sellAmount { get; set; }
        public string sellAmountHover { get; set; }
        public double sellAmountSort { get; set; }
        public bool sellAmountSubscript { get; set; }
        public long seq { get; set; }
        public string source { get; set; }
        public string sourceName { get; set; }
        public string specialClause { get; set; }
        public string specialSort { get; set; }
        public double spread { get; set; }
        public string thsCode { get; set; }
        public string time { get; set; }
        public long timeSort { get; set; }
        public string vacation { get; set; }
        public string valuationDate { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
