package com.meownetstudios.currencyconverter;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

public class MainActivity extends AppCompatActivity {

    public void convert(View view){

        //create a variable for the dollarAmount
        EditText dollarAmountEditText = (EditText) findViewById(R.id.dollarAmountEditText);

        //parse the string to a double
        Double dollarAmountDouble = Double.parseDouble(dollarAmountEditText.getText().toString());

        Double poundAmount = dollarAmountDouble * 0.75;

        Toast.makeText(this,"£" + String.format("%.2f", poundAmount), Toast.LENGTH_LONG).show();


        Log.i("amount",dollarAmountEditText.getText().toString());

    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }
}
