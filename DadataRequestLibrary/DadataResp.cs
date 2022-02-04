namespace DadataRequestLibrary
{

    public class Rootobject
    {
        public Suggestion[] suggestions { get; set; }
    }

    public class Suggestion
    {
        public string value { get; set; }
        public string unrestricted_value { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string kpp { get; set; }
        public object capital { get; set; }
        public Management management { get; set; }
        public object founders { get; set; }
        public object managers { get; set; }
        public object predecessors { get; set; }
        public object successors { get; set; }
        public string branch_type { get; set; }
        public int branch_count { get; set; }
        public object source { get; set; }
        public object qc { get; set; }
        public string hid { get; set; }
        public string type { get; set; }
        public State state { get; set; }
        public Opf opf { get; set; }
        public Name name { get; set; }
        public string inn { get; set; }
        public string ogrn { get; set; }
        public string okpo { get; set; }
        public string okato { get; set; }
        public string oktmo { get; set; }
        public string okogu { get; set; }
        public string okfs { get; set; }
        public string okved { get; set; }
        public object okveds { get; set; }
        public object authorities { get; set; }
        public object documents { get; set; }
        public object licenses { get; set; }
        public object finance { get; set; }
        public Address address { get; set; }
        public object phones { get; set; }
        public object emails { get; set; }
        public long? ogrn_date { get; set; }
        public string okved_type { get; set; }
        public object employee_count { get; set; }
    }

    public class Management
    {
        public string name { get; set; }
        public string post { get; set; }
        public object disqualified { get; set; }
    }

    public class State
    {
        public string status { get; set; }
        public object code { get; set; }
        public long actuality_date { get; set; }
        public long? registration_date { get; set; }
        public object liquidation_date { get; set; }
    }

    public class Opf
    {
        public string type { get; set; }
        public string code { get; set; }
        public string full { get; set; }
        public string _short { get; set; }
    }

    public class Name
    {
        public string full_with_opf { get; set; }
        public string short_with_opf { get; set; }
        public object latin { get; set; }
        public string full { get; set; }
        public string _short { get; set; }
    }

    public class Address
    {
        public string value { get; set; }
        public string unrestricted_value { get; set; }
        public Data1 data { get; set; }
    }

    public class Data1
    {
        public string postal_code { get; set; }
        public string country { get; set; }
        public string country_iso_code { get; set; }
        public string federal_district { get; set; }
        public string region_fias_id { get; set; }
        public string region_kladr_id { get; set; }
        public string region_iso_code { get; set; }
        public string region_with_type { get; set; }
        public string region_type { get; set; }
        public string region_type_full { get; set; }
        public string region { get; set; }
        public object area_fias_id { get; set; }
        public object area_kladr_id { get; set; }
        public object area_with_type { get; set; }
        public object area_type { get; set; }
        public object area_type_full { get; set; }
        public object area { get; set; }
        public string city_fias_id { get; set; }
        public string city_kladr_id { get; set; }
        public string city_with_type { get; set; }
        public string city_type { get; set; }
        public string city_type_full { get; set; }
        public string city { get; set; }
        public string city_area { get; set; }
        public object city_district_fias_id { get; set; }
        public object city_district_kladr_id { get; set; }
        public string city_district_with_type { get; set; }
        public string city_district_type { get; set; }
        public string city_district_type_full { get; set; }
        public string city_district { get; set; }
        public object settlement_fias_id { get; set; }
        public object settlement_kladr_id { get; set; }
        public object settlement_with_type { get; set; }
        public object settlement_type { get; set; }
        public object settlement_type_full { get; set; }
        public object settlement { get; set; }
        public string street_fias_id { get; set; }
        public string street_kladr_id { get; set; }
        public string street_with_type { get; set; }
        public string street_type { get; set; }
        public string street_type_full { get; set; }
        public string street { get; set; }
        public string house_fias_id { get; set; }
        public string house_kladr_id { get; set; }
        public object house_cadnum { get; set; }
        public string house_type { get; set; }
        public string house_type_full { get; set; }
        public string house { get; set; }
        public string block_type { get; set; }
        public string block_type_full { get; set; }
        public string block { get; set; }
        public object entrance { get; set; }
        public object floor { get; set; }
        public object flat_fias_id { get; set; }
        public object flat_cadnum { get; set; }
        public string flat_type { get; set; }
        public string flat_type_full { get; set; }
        public string flat { get; set; }
        public string flat_area { get; set; }
        public object square_meter_price { get; set; }
        public object flat_price { get; set; }
        public object postal_box { get; set; }
        public string fias_id { get; set; }
        public string fias_code { get; set; }
        public string fias_level { get; set; }
        public string fias_actuality_state { get; set; }
        public string kladr_id { get; set; }
        public string geoname_id { get; set; }
        public string capital_marker { get; set; }
        public string okato { get; set; }
        public string oktmo { get; set; }
        public string tax_office { get; set; }
        public string tax_office_legal { get; set; }
        public string timezone { get; set; }
        public string geo_lat { get; set; }
        public string geo_lon { get; set; }
        public string beltway_hit { get; set; }
        public object beltway_distance { get; set; }
        public Metro[] metro { get; set; }
        public string qc_geo { get; set; }
        public object qc_complete { get; set; }
        public object qc_house { get; set; }
        public object history_values { get; set; }
        public object unparsed_parts { get; set; }
        public string source { get; set; }
        public string qc { get; set; }
    }

    public class Metro
    {
        public string name { get; set; }
        public string line { get; set; }
        public float distance { get; set; }
    }

}
