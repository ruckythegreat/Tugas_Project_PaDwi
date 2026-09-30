package com.example.smkrestaurantapp

import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class MainActivity : AppCompatActivity() {

    private lateinit var rvMenu: RecyclerView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_main)

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main)) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom)
            insets
        }

        rvMenu = findViewById(R.id.rvMenu)
        rvMenu.layoutManager = LinearLayoutManager(this)

        fetchMenuData()
    }

    private fun fetchMenuData() {
        val baseUrl = "http://192.168.1.7:5023/"

        val retrofit = Retrofit.Builder()
            .baseUrl(baseUrl)
            .addConverterFactory(GsonConverterFactory.create())
            .build()

        val apiService = retrofit.create(ApiService::class.java)

        val token = "Bearer token_akses_kamu_di_sini"
        apiService.getMenu(token).enqueue(object : Callback<List<Menu>> {
            override fun onResponse(call: Call<List<Menu>>, response: Response<List<Menu>>) {
                if (response.isSuccessful) {
                    val menuList = response.body() ?: emptyList()
                    rvMenu.adapter = MenuAdapter(menuList)
                } else {
                    Toast.makeText(this@MainActivity, "Gagal memuat data: ${response.code()}", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<List<Menu>>, t: Throwable) {
                Toast.makeText(this@MainActivity, "Error Koneksi: ${t.message}", Toast.LENGTH_SHORT).show()
            }
        })
    }
}
