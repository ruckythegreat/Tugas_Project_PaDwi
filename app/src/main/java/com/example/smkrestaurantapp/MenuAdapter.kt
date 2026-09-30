package com.example.smkrestaurantapp

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView

class MenuAdapter(private val menuList: List<Menu>) :
    RecyclerView.Adapter<MenuAdapter.MenuViewHolder>() {

    class MenuViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val tvName: TextView = itemView.findViewById(R.id.tvMenuName)
        val tvDescription: TextView = itemView.findViewById(R.id.tvMenuDescription)
        val tvPrice: TextView = itemView.findViewById(R.id.tvMenuPrice)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MenuViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_menu, parent, false)
        return MenuViewHolder(view)
    }

    override fun onBindViewHolder(holder: MenuViewHolder, position: Int) {
        val item = menuList[position]
        holder.tvName.text = item.name
        holder.tvDescription.text = item.description
        holder.tvPrice.text = "Rp ${item.price}"
    }

    override fun getItemCount(): Int = menuList.size
}
