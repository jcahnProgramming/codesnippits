package com.meownetstudios.demoapp;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.Toast;

public class MainActivity extends AppCompatActivity {

public void ChangeCatFunction(View view){

    Log.i("Test","Button Clicked");

    ImageView image = (ImageView) findViewById(R.id.catImageView);
    image.setImageResource(R.drawable.cat2);


    //ImageView img = new ImageView(this);
    //img.setImageResource(R.drawable.cat2);

}

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }
}
