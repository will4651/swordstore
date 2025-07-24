package wharris.SWORD100CartServiceAPI;

import org.springframework.boot.autoconfigure.data.redis.RedisProperties;
import org.springframework.boot.context.properties.EnableConfigurationProperties;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.redis.connection.RedisConnectionFactory;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.data.redis.repository.configuration.EnableRedisRepositories;
import org.springframework.data.redis.repository.configuration.*;

@Configuration
@EnableConfigurationProperties(RedisProperties.class)
@EnableRedisRepositories
public class BeanConfiguration {

    @Bean
    public RedisTemplate<String, Cart> redisTemplate(RedisConnectionFactory connectionFactory) {
        RedisTemplate<String, Cart> template = new RedisTemplate<>();
        template.setConnectionFactory(connectionFactory);
        return template;
    }
}