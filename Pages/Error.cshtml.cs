using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace DW_26256_27229.Pages;
[ResponseCache(Duration=0,Location=ResponseCacheLocation.None,NoStore=true)]
public class ErrorModel:PageModel{
public int? StatusCode{get;set;}
public void OnGet(int? id){
StatusCode=id;
}
}
