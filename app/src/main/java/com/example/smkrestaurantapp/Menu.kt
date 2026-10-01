package com.example.smkrestaurantapp

import com.google.gson.annotations.SerializedName

data class Menu(
    @SerializedName("name")
    val name: String,

    @SerializedName("description")
    val description: String,

    @SerializedName("price")
    val price: Int
)
