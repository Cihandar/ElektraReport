using ElektraReport.Interfaces.ElektraApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using ElektraReport.Interfaces.Cruds;
using ElektraReport.Models.ApiModels;
using ElektraReport.Models.ReportModels;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using ElektraReport.Models.App;

namespace ElektraReport.Applications.ElektraApis.Command
{
    public class ApiRequest : IApiRequest
    {
        ICompanyCrud _companyCrud;
        public ApiRequest(ICompanyCrud companyCrud)
        {
            _companyCrud = companyCrud;
        }

        //Günlük Durum Toplamları
        public async Task<ApiResults<PosForecast>> GetPosForecast(Guid CompanyId, DateTime date,DateTime date2)
        {
            var company = await _companyCrud.GetById(CompanyId);
            var query = string.Format(@"SELECT SUM(CASE WHEN MASANO LIKE 'Z%' THEN ONAKIT ELSE 0 END) AS NakitToplam,
                SUM(CASE WHEN MASANO LIKE 'Z%' AND ONAKIT > 0 THEN 1 ELSE 0 END) AS NakitAdet,  
                SUM(CASE WHEN MASANO LIKE 'Z%' THEN OKK ELSE 0 END) AS KkToplam,
                SUM(CASE WHEN  MASANO LIKE 'Z%' AND OKK > 0 THEN 1 ELSE 0 END) AS KkAdet,    
                SUM(CASE WHEN MASANO LIKE 'Z%' THEN OACIK ELSE 0 END) AS OToplam,  
                SUM(CASE WHEN  MASANO LIKE 'Z%' AND OACIK > 0 THEN 1 ELSE 0 END) AS OAdet,
                SUM(CASE WHEN MASANO LIKE 'Z%' AND ONAKIT > 0 THEN KISI ELSE 0 END) AS NKisi,        
                SUM(CASE WHEN  MASANO LIKE 'Z%' AND OKK > 0 THEN KISI ELSE 0 END) AS KKisi, 
                SUM(CASE WHEN  MASANO LIKE 'Z%' AND OACIK > 0 THEN KISI ELSE 0 END) AS OKisi,
                SUM(CASE WHEN MUSTERIID > 0 THEN OACIK ELSE 0 END) AS CariTutar,
                SUM(CASE WHEN MUSTERIID > 0 THEN 1 ELSE 0 END) AS CariAdet, 
                SUM(CASE WHEN MASANO NOT LIKE 'Z%' THEN TOPLAM ELSE 0.00 END) AS AcikMasaToplam,
                SUM(CASE WHEN MASANO NOT LIKE 'Z%' THEN 1 ELSE 0 END) AS AcikMasaAdet,
                SUM(CASE WHEN MASANO NOT LIKE 'Z%' THEN KISI ELSE 0 END) AS AcikMasaKisi,
                SUM(CASE WHEN MASANO LIKE 'Z%' THEN 1 ELSE 0 END) AS ToplamAdet,
                SUM(CASE WHEN MASANO LIKE 'Z%' THEN KISI ELSE 0 END) AS ToplamKisi,
                (SELECT SUM(ALACAK)  FROM CARIISL CI WHERE CI.TARIH BETWEEN '{0}' AND '{1}' AND CARIID2 = 17)  AS NkCariTahsilat,
                (SELECT SUM(ALACAK)  FROM CARIISL CI WHERE CI.TARIH BETWEEN '{0}' AND '{1}' AND CARIID2 = 9)  AS KkCariTahsilat,
                SUM(CASE WHEN MASANO LIKE 'Z%' THEN TOPLAM ELSE 0 END) SatisToplam,
                SUM(CASE WHEN MASANO LIKE 'Z%' THEN TOPLAM ELSE 0 END) + (SELECT SUM(ALACAK)  FROM CARIISL CI WHERE CI.TARIH BETWEEN '{0}' AND '{1}')  AS  KasaToplam
                FROM FISARSIV F
                WHERE TARIH BETWEEN '{0}' AND '{1}'", date.ToString("yyyy-MM-dd"),date2.ToString("yyyy-MM-dd"));
            /*(SELECT SUM(ALACAK)  FROM CARIISL CI WHERE CI.TARIH = F.TARIH AND CARIID2 = 17)  AS NKCARITAHSILAT,
                (SELECT SUM(ALACAK)  FROM CARIISL CI WHERE CI.TARIH = F.TARIH AND CARIID2 = 9)  AS KKCARITAHSILAT,*/
            var result = await company.ApiUrl.PostJsonAsync(new
            {
                Query = query,
                userName = company.Username,
                password = company.Password
            }).ReceiveJson<ApiResults<PosForecast>>();

            if (result.Status)
            {
                try
                {
                    
                    var resultData = JsonConvert.DeserializeObject<List<PosForecast>>(result.Result.ToString());
                    if (resultData == null) resultData = new List<PosForecast>();
                    result.Data = resultData;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "Deserialize Error : " + ex.Message;
                }
            }

            return result;
        }
       
        //Satış Raporu
        public async Task<ApiResults<PosSaleReport>> GetPosSaleReport(Guid CompanyId, DateTime date, DateTime date2)
        {
            var company = await _companyCrud.GetById(CompanyId);
            var query = string.Format(@"SELECT Q.TARIH Tarih,Q.ASAATI ASaati,Q.KSAATI KSaati,Q.KISI Kisi,Q.FISCOUNTER Fiscounter, Q.FISNO Fisno,Q.TUR AS Islem,Q.MASANO Masano,GARSONADI Garson, F.INDTUTAR IndTutar,Q.OACIK Kredi,Q.OKK KKarti,Q.ONAKIT Nakit,Q.TOPLAM Toplam
                                        FROM QMUH_ADISYON Q LEFT JOIN FISARSIV F ON  Q.FISCOUNTER=F.FISCOUNTER 
                                        WHERE 1=1 AND Q.TARIH BETWEEN '{0}' AND '{1}' AND Q.TUR='SATIS'
                                        ORDER BY Q.TARIH,Q.FISCOUNTER", date.ToString("yyyy-MM-dd"), date2.ToString("yyyy-MM-dd"));

            var result = await company.ApiUrl.PostJsonAsync(new
            {
                Query = query,
                userName = company.Username,
                password = company.Password
            }).ReceiveJson<ApiResults<PosSaleReport>>();

            if (result.Status)
            {
                try
                {
                    var resultData = JsonConvert.DeserializeObject<List<PosSaleReport>>(result.Result.ToString());
                    result.Data = resultData;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "Deserialize Error : " + ex.Message;
                }
            }

            return result;
        }

        //İndirim Raporu
        public async Task<ApiResults<PosDicountReport>> GetPosDiscountReport(Guid CompanyId, DateTime date, DateTime date2)
        {
            var company = await _companyCrud.GetById(CompanyId);
            var query = string.Format(@"SELECT TARIH Tarih,FISCOUNTER FisCounter, FISNO Fisno,TUR Islem,MASANO MasaNo,G.GADI Garson ,INDTUTAR IndTutar 
                                        FROM FISARSIV A INNER JOIN GARSON G  ON G.GKODU=A.GARSONKODU 
                                        WHERE  INDTUTAR>0 AND TUR='SATIS' AND TARIH BETWEEN '{0}' AND '{1}' 
                                        ORDER BY TARIH,FISCOUNTER", date.ToString("yyyy-MM-dd"), date2.ToString("yyyy-MM-dd"));

            var result = await company.ApiUrl.PostJsonAsync(new
            {
                Query = query,
                userName = company.Username,
                password = company.Password
            }).ReceiveJson<ApiResults<PosDicountReport>>();

            if (result.Status)
            {
                try
                {
                    var resultData = JsonConvert.DeserializeObject<List<PosDicountReport>>(result.Result.ToString());
                    result.Data = resultData;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "Deserialize Error : " + ex.Message;
                }
            }

            return result;
        }

        //Ödenmezler Raporu
        public async Task<ApiResults<PosNotPayReport>> GetPosNotPayReport(Guid CompanyId, DateTime date, DateTime date2)
        {
            var company = await _companyCrud.GetById(CompanyId);
            var query = string.Format(@"SELECT   TARIH Tarih,FISCOUNTER FisCounter, FISNO FisNo,TUR Islem,MASANO MasaNo,G.GADI Garson,MUSTERIADI MusteriAdi,TOPLAM Toplam 
                                        FROM FISARSIV A INNER JOIN GARSON G  ON G.GKODU=A.GARSONKODU 
                                        WHERE TUR<>'SATIS' AND TARIH BETWEEN '{0}' AND '{1}'
                                        ORDER BY TARIH,FISCOUNTER", date.ToString("yyyy-MM-dd"), date2.ToString("yyyy-MM-dd"));

            var result = await company.ApiUrl.PostJsonAsync(new
            {
                Query = query,
                userName = company.Username,
                password = company.Password
            }).ReceiveJson<ApiResults<PosNotPayReport>>();

            if (result.Status)
            {
                try
                {
                    var resultData = JsonConvert.DeserializeObject<List<PosNotPayReport>>(result.Result.ToString());
                    result.Data = resultData;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "Deserialize Error : " + ex.Message;
                }
            }

            return result;
        }

        //Krediye Kaldırılanlar Raporu (Cari Raporu)
        public async Task<ApiResults<PosCreditReport>> GetPosCreditReport(Guid CompanyId, DateTime date, DateTime date2)
        {
            var company = await _companyCrud.GetById(CompanyId);
            var query = string.Format(@"SELECT   TARIH Tarih,FISCOUNTER FisCounter, FISNO FisNo,TUR Islem,MASANO MasaNo,G.GADI Garson,MUSTERIADI MusteriAdi,TOPLAM Toplam 
                                        FROM FISARSIV A INNER JOIN GARSON G  ON G.GKODU=A.GARSONKODU 
                                        WHERE OACIK>0 AND TUR='SATIS' AND TARIH BETWEEN '{0}' AND '{1}'
                                        ORDER BY TARIH,FISCOUNTER", date.ToString("yyyy-MM-dd"), date2.ToString("yyyy-MM-dd"));

            var result = await company.ApiUrl.PostJsonAsync(new
            {
                Query = query,
                userName = company.Username,
                password = company.Password
            }).ReceiveJson<ApiResults<PosCreditReport>>();

            if (result.Status)
            {
                try
                {
                    var resultData = JsonConvert.DeserializeObject<List<PosCreditReport>>(result.Result.ToString());
                    result.Data = resultData;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "Deserialize Error : " + ex.Message;
                }
            }

            return result;
        }

        //İptal Edilen Siparişler Raporu
        public async Task<ApiResults<PosCancelReport>> GetPosCancelReport(Guid CompanyId, DateTime date, DateTime date2)
        {
            var company = await _companyCrud.GetById(CompanyId);
            var query = string.Format(@"SELECT I.TARIH AS Tarih,S.ADI UrunAdi,SAAT Saat,SIPSAAT SipSaat,CAST(SAAT AS DATETIME)-CAST(SIPSAAT AS DATETIME) AS Sure  ,ADET Adet,I.MASANO MasaNo,TUR Tur,MUSTERIADI MusteriAdi,I.FISCOUNTER Fisno,(SELECT GADI FROM GARSON G WHERE                             G.GKODU=I.GARSONKODU) GarsonAdi,I.ACIKLAMA Aciklama,BIRIMFIYAT*ADET AS Tutar
                                        FROM ADSIADE I INNER JOIN STOK S ON S.STOKID=I.STOKID INNER JOIN FISARSIV FA ON I.FISCOUNTER=FA.FISCOUNTER  
                                        WHERE FA.TARIH BETWEEN '{0}' AND '{1}'
                                        ORDER BY I.TARIH,I.FISCOUNTER", date.ToString("yyyy-MM-dd"), date2.ToString("yyyy-MM-dd"));

            var result = await company.ApiUrl.PostJsonAsync(new
            {
                Query = query,
                userName = company.Username,
                password = company.Password
            }).ReceiveJson<ApiResults<PosCancelReport>>();

            if (result.Status)
            {
                try
                {
                    var resultData = JsonConvert.DeserializeObject<List<PosCancelReport>>(result.Result.ToString());
                    result.Data = resultData;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "Deserialize Error : " + ex.Message;
                }
            }

            return result;
        }

        //Ödenmez için Detaylı Rapor
        public async Task<ApiResults<PosNotPayDetailReport>> GetPosNotPayDetailReport(Guid CompanyId, DateTime date, DateTime date2)
        {
            var company = await _companyCrud.GetById(CompanyId);
            var query = string.Format(@"SELECT TARIH Tarih,STOKADI StokAdi,SUM(ADET) Adet,SUM(ADET*NFIYAT)  AS Tutar,CARIUNVANI CariUnvani
                                        FROM QMUH_ADISYONISLEM 
                                        WHERE TUR<>'SATIS' AND TARIH BETWEEN '{0}' AND '{1}'                                 
                                        GROUP BY TARIH,STOKADI,CARIUNVANI
                                        ORDER BY CARIUNVANI,STOKADI", date.ToString("yyyy-MM-dd"), date2.ToString("yyyy-MM-dd"));

            var result = await company.ApiUrl.PostJsonAsync(new
            {
                Query = query,
                userName = company.Username,
                password = company.Password
            }).ReceiveJson<ApiResults<PosNotPayDetailReport>>();

            if (result.Status)
            {
                try
                {
                    var resultData = JsonConvert.DeserializeObject<List<PosNotPayDetailReport>>(result.Result.ToString());
                    result.Data = resultData;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "Deserialize Error : " + ex.Message;
                }
            }

            return result;
        }

        //Ürün Satış Raporu
        public async Task<ApiResults<PosProductSalesReport>> GetPosProductSalesReport(Guid CompanyId, DateTime date, DateTime date2)
        {
            var company = await _companyCrud.GetById(CompanyId);
            var query = string.Format(@"SELECT TARIH Tarih,STOKADI StokAdi,SUM(ADET) Adet,SUM(ADET*NFIYAT) AS Tutar
                                        FROM QMUH_ADISYONISLEM 
                                        WHERE TUR='SATIS' AND TARIH BETWEEN '{0}' AND '{1}'                                  
                                        GROUP BY TARIH,STOKADI
                                        ORDER BY TARIH,ADET DESC", date.ToString("yyyy-MM-dd"), date2.ToString("yyyy-MM-dd"));

            var result = await company.ApiUrl.PostJsonAsync(new
            {
                Query = query,
                userName = company.Username,
                password = company.Password
            }).ReceiveJson<ApiResults<PosProductSalesReport>>();

            if (result.Status)
            {
                try
                {
                    var resultData = JsonConvert.DeserializeObject<List<PosProductSalesReport>>(result.Result.ToString());
                    result.Data = resultData;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "Deserialize Error : " + ex.Message;
                }
            }

            return result;
        }

        //Son 7 Gün Ciro Raporu (Grafik İçin)
        public async Task<ApiResults<PosChartReport>> GetPosChartReport(Guid CompanyId, DateTime date, DateTime date2)
        {
            var company = await _companyCrud.GetById(CompanyId);
            var query = string.Format(@"SELECT TARIH Tarih,
                SUM(CASE WHEN MASANO LIKE 'Z%' THEN TOPLAM ELSE 0 END) + (SELECT SUM(ALACAK)  FROM CARIISL CI WHERE CI.TARIH BETWEEN '{0}' AND '{1}' )  AS  Toplam
                FROM FISARSIV F
                WHERE TARIH BETWEEN '{0}' AND '{1}'  group by TARIH", date.ToString("yyyy-MM-dd"), date2.ToString("yyyy-MM-dd"));

            var result = await company.ApiUrl.PostJsonAsync(new
            {
                Query = query,
                userName = company.Username,
                password = company.Password
            }).ReceiveJson<ApiResults<PosChartReport>>();

            if (result.Status)
            {
                try
                {
                    var resultData = JsonConvert.DeserializeObject<List<PosChartReport>>(result.Result.ToString());
                    result.Data = resultData;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "Deserialize Error : " + ex.Message;
                }
            }

            return result;
        }

        //Açık Masalar Sorgusu
        public async Task<ApiResults<PosOpenTables>> GetPosOpenTables(Guid CompanyId)
        {
            var company = await _companyCrud.GetById(CompanyId);
            var query = string.Format(@"SELECT TARIH Tarih,MASANO MasaNo,ASAATI ASaati,KSAATI KSaati,G.GADI Garson,TOPLAM Toplam,FISCOUNTER Id FROM ADISYON A INNER JOIN GARSON G ON G.GKODU=A.GARSONKODU WHERE MASANO NOT LIKE 'Z%' ORDER BY MasaNo");

            var result = await company.ApiUrl.PostJsonAsync(new
            {
                Query = query,
                userName = company.Username,
                password = company.Password
            }).ReceiveJson<ApiResults<PosOpenTables>>();

            if (result.Status)
            {
                try
                {
                    var resultData = JsonConvert.DeserializeObject<List<PosOpenTables>>(result.Result.ToString());
                    result.Data = resultData;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "Deserialize Error : " + ex.Message;
                }
            }

            return result;
        }

        //Açık Masalar için Detay Sorgusu (Adisyon Satırı)
        public async Task<ApiResults<PosOpenTablesDetails>> GetPosOpenTablesDetails(Guid CompanyId,int Id)
        {
            var company = await _companyCrud.GetById(CompanyId);
            var query = string.Format(@"SELECT ST.ADI UrunAdi,ADET Adet,S.BIRIMFIYAT BFiyat,S.BIRIMFIYAT*S.ADET Toplam,A.TARIH Tarih,A.ASAATI ASaati,A.TOPLAM GenelToplam,G.GADI Garson  FROM ADSSATIR S INNER JOIN ADISYON A ON S.FISCOUNTER=A.FISCOUNTER INNER JOIN STOK ST ON ST.STOKID=S.STOKID LEFT  JOIN GARSON G ON G.GKODU=A.GARSONKODU  WHERE S.FISCOUNTER={0}", Id);

            var result = await company.ApiUrl.PostJsonAsync(new
            {
                Query = query,
                userName = company.Username,
                password = company.Password
            }).ReceiveJson<ApiResults<PosOpenTablesDetails>>();

            if (result.Status)
            {
                try
                {
                    var resultData = JsonConvert.DeserializeObject<List<PosOpenTablesDetails>>(result.Result.ToString());
                    result.Data = resultData;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "Deserialize Error : " + ex.Message;
                }
            }

            return result;
        }

        //Sipariş Takibi Sorgusu
        public async Task<ApiResults<PosOrders>> GetPosOrders(Guid CompanyId)
        {
            var company = await _companyCrud.GetById(CompanyId);
            var query = string.Format(@"SELECT * FROM (
				SELECT 'Satış' as Tur, ADS.COUNTER SatirId,A.FISCOUNTER FisId,S.ADI UrunAdi,ADS.ADET Adet,ADS.SAAT SiparisSaati,G.GADI Garson,A.ASAATI MasaAcilisSaati,A.MASANO MasaNo,ADS.ACIKLAMA Aciklama
				FROM ADSSATIR ADS 
				INNER JOIN ADISYON A ON A.FISCOUNTER=ADS.FISCOUNTER 
				INNER JOIN STOK S ON S.STOKID=ADS.STOKID
				LEFT JOIN GARSON G ON ADS.GARSONKODU=G.GKODU
				WHERE A.MASANO NOT LIKE 'Z%' 
				UNION ALL				
				SELECT 'İptal' as Tur,ADS.COUNTER SatirId,A.FISCOUNTER FisId,S.ADI UrunAdi,ADS.ADET Adet,ADS.SAAT SiparisSaati,G.GADI Garson,A.ASAATI MasaAcilisSaati,A.MASANO MasaNo,ADS.ACIKLAMA Aciklama
				FROM ADSIADE ADS 
				INNER JOIN ADISYON A ON A.FISCOUNTER=ADS.FISCOUNTER 
				INNER JOIN STOK S ON S.STOKID=ADS.STOKID
				LEFT JOIN GARSON G ON ADS.GARSONKODU=G.GKODU
				WHERE A.MASANO NOT LIKE 'Z%') AS A WHERE 1=1
				ORDER BY SiparisSaati DESC");

            var result = await company.ApiUrl.PostJsonAsync(new
            {
                Query = query,
                userName = company.Username,
                password = company.Password
            }).ReceiveJson<ApiResults<PosOrders>>();

            if (result.Status)
            {
                try
                {
                    var resultData = JsonConvert.DeserializeObject<List<PosOrders>>(result.Result.ToString());
                    result.Data = resultData;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "Deserialize Error : " + ex.Message;
                }
            }

            return result;
        }

    }
}
