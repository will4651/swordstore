package wharris.SWORD100ServiceAPI;
import java.util.List;
import java.util.UUID;

import org.springframework.data.mongodb.repository.MongoRepository;

public interface SwordRepository extends MongoRepository<Sword, UUID> { // second param used to be long when Id was long, see Book class

    public List<Sword> findByNameContainingOrDescriptionContaining(String txt, String txt2);

}
