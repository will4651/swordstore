package wharris.SWORD100CartServiceAPI;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.client.discovery.EnableDiscoveryClient;

@SpringBootApplication
@EnableDiscoveryClient
public class Sword100CartServiceApiApplication {

	public static void main(String[] args) {
		SpringApplication.run(Sword100CartServiceApiApplication.class, args);
	}

}
