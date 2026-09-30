package com.example.smkrestaurantapp

import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Header

interface ApiService {

    @GET("menu")
    fun getMenu(
        @Header("Authorization") token: String
    ): Call<List<Menu>>
}
